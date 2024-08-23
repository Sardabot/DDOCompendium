using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace DDOCompendium
{
    public partial class Main : Form
    {
        public string DataFolderPath = Application.ExecutablePath.Replace("DDOCompendium.exe", "Data/");
        public string QuestsFilePath;
        public string CharactersFilePath;
        public DataTable questsTable;
        public DataView questsDataView;
        public Dictionary<string, Character> characterData;
        public List<DataTable> SagaTables = [];
        public List<Saga> sagaData;
        public bool SagasFormatted = false;
        public string SelectedCharacterName = Properties.Settings.Default.SelectedCharacterName;
        public string SelectedDifficulty = "Elite";
        public string LevelFilter = "All";
        private DataGridViewCell ClickedCell;
        public DataGridViewCellStyle darkgridcellstyle = new()
        {
            BackColor = Color.FromArgb(64, 64, 64),
            ForeColor = Color.FromArgb(224, 224, 244)
        };
        public DataGridViewCellStyle sagaNAcellstyle = new()
        {
            BackColor = SystemColors.ControlDarkDark,
            ForeColor = SystemColors.ControlDarkDark
        };

        public Main()
        {
            InitializeComponent();

            if (!ImportData()) Application.Exit();
        }

        private bool ImportData()
        {
            Text = "DDO Compendium - " + SelectedCharacterName;
            QuestsFilePath = DataFolderPath + "Quests.json";
            string importedJsonData = ReadFromFile(QuestsFilePath);
            var importedQuestData = JsonConvert.DeserializeObject<List<QuestPack>>(importedJsonData);
            if (importedQuestData is null)
            {
                MessageBox.Show("Couldn't retrieve data from Quests.json");
                return false;
            }
            // import user save data
            CharactersFilePath = DataFolderPath + "Characters.json";
            importedJsonData = ReadFromFile(CharactersFilePath);
            characterData = JsonConvert.DeserializeObject<Dictionary<string, Character>>(importedJsonData);
            if (characterData is null)
            {
                MessageBox.Show("Couldn't retrieve data from Characters.json");
                return false;
            }

            // unpack these into a table of quests
            questsTable = MakeQuestsTable();
            foreach (QuestPack thisPack in importedQuestData)
            {
                foreach (Quest thisQuest in thisPack.Quests)
                {
                    DataRow tempRow = questsTable.NewRow();
                    string tempID = thisPack.Id.ToString() + "-" + thisQuest.Id.ToString();
                    tempRow.SetField("ID", tempID);
                    tempRow.SetField("Wiki Name", thisQuest.WikiName);
                    tempRow.SetField("H", thisQuest.HeroicLevel);
                    tempRow.SetField("E", thisQuest.EpicLevel);
                    tempRow.SetField("L", thisQuest.LegLevel);
                    tempRow.SetField("Name", thisQuest.Name);
                    tempRow.SetField("Pack", thisPack.Name);
                    tempRow.SetField("Character", "");
                    tempRow.SetField("Patron", thisQuest.Patron);
                    tempRow.SetField("Favor", thisQuest.Favor);
                    tempRow.SetField("Style", thisQuest.Style);
                    tempRow.SetField("SortWithPack", thisQuest.SortWithPack);
                    questsTable.Rows.Add(tempRow);
                }
            }
            questsDataView = new DataView(questsTable);
            BindingSource questsDataSource = new()
            {
                DataSource = questsDataView
            };
            datagridQuests.DataSource = questsDataSource;
            // make adjustments to grid settings
            datagridQuests.Columns[QUESTSGRID_ID_INDEX].Visible = false;
            datagridQuests.Columns[QUESTSGRID_WIKI_INDEX].Visible = false;
            datagridQuests.Columns[QUESTSGRID_EPIC_INDEX].SortMode = DataGridViewColumnSortMode.NotSortable;
            datagridQuests.Columns[QUESTSGRID_LEGENDARY_INDEX].SortMode = DataGridViewColumnSortMode.NotSortable;
            datagridQuests.Columns[QUESTSGRID_COMPLETED_INDEX].SortMode = DataGridViewColumnSortMode.NotSortable;
            datagridQuests.Columns[QUESTSGRID_COMPLETED_INDEX].HeaderText = SelectedCharacterName;
            datagridQuests.Columns[QUESTSGRID_PACKSORT_INDEX].Visible = false;

            // import the notes tabs
            txtNotes1.Text = ReadFromFile(DataFolderPath + "Notes.txt");
            txtNotes2.Text = ReadFromFile(DataFolderPath + "Notes2.txt");

            // import ref tables

            // import wilderness tables

            // setup characters tab

            // import saga tables
            importedJsonData = ReadFromFile(DataFolderPath + "Sagas.json");
            sagaData = JsonConvert.DeserializeObject<List<Saga>>(importedJsonData);
            sagaData.Sort(delegate (Saga x, Saga y)
            {
                if (x.SortLevel == y.SortLevel) return 0;
                else if (x.SortLevel < y.SortLevel) return -1;
                else return 1;
            });
            foreach (Saga thisSagaData in sagaData)
            {
                DataTable table = MakeSagaTable(thisSagaData.Quests);
                SagaTables.Add(table);
                DataGridView thisdgview = new() {
                    AllowUserToAddRows = false,
                    AllowUserToDeleteRows = false,
                    AllowUserToResizeRows = false,
                    AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells,
                    BackgroundColor = SystemColors.ControlDarkDark,
                    ColumnHeadersDefaultCellStyle = darkgridcellstyle,
                    DefaultCellStyle = darkgridcellstyle,
                    RowHeadersVisible = false,
                    EnableHeadersVisualStyles = false,
                    DataSource = new BindingSource { DataSource = table },
                    Dock = DockStyle.Fill,
                    ScrollBars = ScrollBars.None,
                    ReadOnly = true,
                    Tag = thisSagaData.Id
                };
                thisdgview.CellMouseClick += DatagridSagas_CellMouseClick;
                tableLayoutPanelSagas.RowCount += 1;
                tableLayoutPanelSagas.Controls.Add(thisdgview, 0, tableLayoutPanelSagas.RowCount - 1);
            }

            // setup settings tab

            // load character data into things
            ChangeSelectedCharacter(SelectedCharacterName);

            return true;
        }

        public const int QUESTSGRID_ID_INDEX = 0;
        public const int QUESTSGRID_WIKI_INDEX = 1;
        public const int QUESTSGRID_HEROIC_INDEX = 2;
        public const int QUESTSGRID_EPIC_INDEX = 3;
        public const int QUESTSGRID_LEGENDARY_INDEX = 4;
        public const int QUESTSGRID_NAME_INDEX = 5;
        public const int QUESTSGRID_PACK_INDEX = 6;
        public const int QUESTSGRID_COMPLETED_INDEX = 7;
        public const int QUESTSGRID_PATRON_INDEX = 8;
        public const int QUESTSGRID_FAVOR_INDEX = 9;
        public const int QUESTSGRID_STYLE_INDEX = 10;
        public const int QUESTSGRID_PACKSORT_INDEX = 11;

        private DataTable MakeQuestsTable()
        {
            DataTable table = new DataTable
            {
                TableName = "Quests"
            };
            table.Columns.Add(new DataColumn("ID", typeof(string)));
            table.Columns.Add(new DataColumn("Wiki Name", typeof(string)));
            table.Columns.Add(new DataColumn("H", typeof(int)));
            table.Columns.Add(new DataColumn("E", typeof(int)));
            table.Columns.Add(new DataColumn("L", typeof(int)));
            table.Columns.Add(new DataColumn("Name", typeof(string)));
            table.Columns.Add(new DataColumn("Pack", typeof(string)));
            table.Columns.Add(new DataColumn("Character", typeof(string)));
            table.Columns.Add(new DataColumn("Patron", typeof(string)));
            table.Columns.Add(new DataColumn("Favor", typeof(int)));
            table.Columns.Add(new DataColumn("Style", typeof(string)));
            table.Columns.Add(new DataColumn("SortWithPack", typeof(string)));

            return table;
        }

        private DataTable MakeSagaTable(string[][] dataIn)
        {
            DataTable table = new DataTable();
            bool firstRow = true;
            foreach (string[] row in dataIn)
            {
                if (firstRow)
                {
                    foreach (string name in row) table.Columns.Add(new DataColumn(name, typeof(string)));
                    firstRow = false;
                }
                else
                {
                    table.Rows.Add(row);
                }
            }
            return table;
        }

        private void FormatSagaGrids()
        {
            foreach (DataGridView thisdgview in tableLayoutPanelSagas.Controls)
            {
                thisdgview.Height = thisdgview.ColumnHeadersHeight + thisdgview.Rows.Cast<DataGridViewRow>().Sum(r => r.Height);
                foreach (DataGridViewColumn thisColumn in thisdgview.Columns)
                {
                    thisColumn.SortMode = DataGridViewColumnSortMode.NotSortable;
                }
                foreach (DataGridViewRow thisRow in thisdgview.Rows)
                {
                    for (int i = 2; i < thisRow.Cells.Count; i++)
                    {
                        if (thisRow.Cells[i].Value.ToString() == "X") thisRow.Cells[i].Style = sagaNAcellstyle;
                    }
                }
            }
            SagasFormatted = true;
        }

        /// <summary>
        /// Updates the various tables and tabs with information for the
        /// newly selected character.  Also updates the column header text to the
        /// name of the newly selected character.
        /// </summary>
        private void ChangeSelectedCharacter(string newChar)
        {
            SelectedCharacterName = newChar;
            LoadQuestDataForCharacter();
            LoadSagaDataForCharacter();
            datagridQuests.Columns[QUESTSGRID_COMPLETED_INDEX].HeaderText = SelectedCharacterName;
            Text = "DDO Compendium - " + SelectedCharacterName;
        }

        private void LoadQuestDataForCharacter()
        {
            foreach (DataRow thisRow in questsTable.Rows)
            {
                thisRow["Character"] = FindQuestCompletionStatus(thisRow["ID"].ToString());
            }
        }

        private void LoadSagaDataForCharacter()
        {
            foreach (DataGridView thisSagadg in tableLayoutPanelSagas.Controls)
            {
                string[] gridIDs = (thisSagadg.Tag as string).Split(',');
                for (int i = 0; i < gridIDs.Count(); i++)
                {
                    characterData[SelectedCharacterName].SagaCompletion.TryGetValue(int.Parse(gridIDs[i]), out List<string> charThisSagaData);
                    if (charThisSagaData == null) charThisSagaData = new List<string>();
                    for (int row = 0; row < thisSagadg.RowCount; row++)
                    {
                        if (row < charThisSagaData.Count) thisSagadg.Rows[row].Cells[2 + i].Value = charThisSagaData[row];
                        else thisSagadg.Rows[row].Cells[2 + i].Value = "";
                    }
                }
            }
        }

        /// <summary>
        /// Searches characterData for the completion status of a quest.
        /// Uses the currently selected character.
        /// </summary>
        /// <param name="questID">The ID to search for, ex. "1-4"</param>
        /// <returns>A string representing the quest difficulty.</returns>
        private string FindQuestCompletionStatus(string questID)
        {
            return characterData[SelectedCharacterName].QuestCompletion.TryGetValue(questID, out var completion) ? completion : "";
        }

        /// <summary>
        /// Updates the text in the Quests datagridview completion column for a quest.
        /// Also updates the associated entry in the characterData object.
        /// </summary>
        /// <param name="thisCell">The affected cell.</param>
        private void ChangeQuestCompletionStatus(DataGridViewCell thisCell)
        {
            string QuestID = datagridQuests.Rows[thisCell.RowIndex].Cells[QUESTSGRID_ID_INDEX].Value.ToString();

            if (thisCell.Value.ToString() == SelectedDifficulty) thisCell.Value = "";
            else if (datagridQuests.Rows[thisCell.RowIndex].Cells[QUESTSGRID_STYLE_INDEX].Value.ToString() == "Solo") thisCell.Value = "Casual";
            else thisCell.Value = SelectedDifficulty;

            if (thisCell.Value.ToString() == "")
            {
                characterData[SelectedCharacterName].QuestCompletion.Remove(QuestID);
            }
            else
            {
                if (characterData[SelectedCharacterName].QuestCompletion.ContainsKey(QuestID))
                {
                    characterData[SelectedCharacterName].QuestCompletion[QuestID] = thisCell.Value.ToString();
                }
                else characterData[SelectedCharacterName].QuestCompletion.Add(QuestID, thisCell.Value.ToString());
            }
        }

        private void ChangeSagaQuestCompletionStatus(int sagaID, DataGridViewCell thisCell)
        {
            if (thisCell.Value.ToString() == SelectedDifficulty) thisCell.Value = "";
            else thisCell.Value = SelectedDifficulty;

            if (!characterData[SelectedCharacterName].SagaCompletion.ContainsKey(sagaID))
            {
                characterData[SelectedCharacterName].SagaCompletion.Add(sagaID, new List<string>());
            }
            while (characterData[SelectedCharacterName].SagaCompletion[sagaID].Count <= thisCell.RowIndex)
            {
                characterData[SelectedCharacterName].SagaCompletion[sagaID].Add("");
            }
            characterData[SelectedCharacterName].SagaCompletion[sagaID][thisCell.RowIndex] = thisCell.Value.ToString();
        }

        /// <summary>
        /// Handles various events that need to happen when some part
        /// of the quests datagridview is clicked on.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DatagridQuests_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            int thisRowIndex = e.RowIndex;
            int thisColumnIndex = e.ColumnIndex;
            if (e.Button == MouseButtons.Left)
            {
                switch (thisColumnIndex)
                {
                    case QUESTSGRID_COMPLETED_INDEX:
                        if (thisRowIndex != -1)
                        {
                            // change completion status of this quest
                            ChangeQuestCompletionStatus(datagridQuests.Rows[thisRowIndex].Cells[thisColumnIndex]);
                        }
                        break;
                    case QUESTSGRID_EPIC_INDEX:
                        // filter the shown quests to epic and above, or remove the filter
                        switch (LevelFilter)
                        {
                            case "All":
                            case "Legendary":
                                LevelFilter = "Epic";
                                questsDataView.RowFilter = "E > 19 OR L > 29";
                                break;
                            case "Epic":
                                LevelFilter = "All";
                                questsDataView.RowFilter = "";
                                break;
                        }
                        break;
                    case QUESTSGRID_LEGENDARY_INDEX:
                        // filter the shown quests to legendary and above, or remove the filter
                        switch (LevelFilter)
                        {
                            case "All":
                            case "Epic":
                                LevelFilter = "Legendary";
                                questsDataView.RowFilter = "L > 29";
                                break;
                            case "Legendary":
                                LevelFilter = "All";
                                questsDataView.RowFilter = "";
                                break;
                        }
                        break;
                    default:
                        break;
                }
            }
        }

        /// <summary>
        /// Handles various events that need to happen when some part
        /// of the quests datagridview is clicked on.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DatagridSagas_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            DataGridView thisdgview = sender as DataGridView;
            int thisRowIndex = e.RowIndex;
            int thisColumnIndex = e.ColumnIndex;
            if (e.Button == MouseButtons.Left)
            {
                switch (thisColumnIndex)
                {
                    case -1:
                    case 0:
                    case 1:
                        break;
                    default:
                        // any other index is going to be a completion cell
                        if (thisRowIndex != -1)
                        {
                            if (thisdgview.Rows[thisRowIndex].Cells[thisColumnIndex].Value.ToString() == "X")
                            {
                                // don't do anything if it's a cell that isn't part of the saga
                                break;
                            }
                            int sagaID = int.Parse((thisdgview.Tag as string).Split(',')[e.ColumnIndex - 2]);
                            // change completion status of this quest
                            ChangeSagaQuestCompletionStatus(sagaID, thisdgview.Rows[thisRowIndex].Cells[thisColumnIndex]);
                        }
                        break;
                }
            }
        }

        private void Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            // ensure saga completion data is updated for the current character
            //UpdateSagaCompletionData();
            // save all our data back to the files
            WriteToFile(CharactersFilePath, JsonConvert.SerializeObject(characterData, Formatting.Indented));
            WriteToFile(DataFolderPath + "Notes.txt", txtNotes1.Text);
            WriteToFile(DataFolderPath + "Notes2.txt", txtNotes2.Text);
        }

        private void WriteToFile(string filePath, string data)
        {
            using StreamWriter writer = new(filePath);
            writer.Write(data);
            writer.Close();
        }

        private string ReadFromFile(string filePath)
        {
            using StreamReader reader = new StreamReader(filePath);
            string tempstr = reader.ReadToEnd();
            reader.Close();
            return tempstr;
        }

        /// <summary>
        /// Provides the appropriate context menu based on which column
        /// of the quests datagridview was right-clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DatagridQuests_CellContextMenuStripNeeded(object sender, DataGridViewCellContextMenuStripNeededEventArgs e)
        {
            if (e.ColumnIndex == QUESTSGRID_COMPLETED_INDEX && e.RowIndex != -1)
            {
                // right-clicked a non-header cell in the completion status column
                // we want to provide options to change the selected difficulty
                e.ContextMenuStrip = contextmenuQuestCompletion;
                ClickedCell = datagridQuests.Rows[e.RowIndex].Cells[e.ColumnIndex];
            }
            else if (e.ColumnIndex == QUESTSGRID_COMPLETED_INDEX && e.RowIndex == -1)
            {
                // right-clicked the header of the completion status column
                // we want to provide options to change the selected character
                e.ContextMenuStrip = contextmenuCharSelect;
                contextmenuCharSelect.Items.Clear();
                foreach (string thisCharName in characterData.Keys)
                {
                    contextmenuCharSelect.Items.Add(thisCharName);
                }
            }
        }

        /// <summary>
        /// Changes the selected difficulty, then calls ChangeQuestCompletionStatus
        /// on the cell the context menu came from to update the value.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ContextmenuQuestCompletion_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            ToolStripItem clickedItem = e.ClickedItem;
            switch (clickedItem.Text)
            {
                case "Clear":
                    SelectedDifficulty = "";
                    break;
                case "Casual":
                case "Normal":
                case "Hard":
                case "Elite":
                    SelectedDifficulty = clickedItem.Text;
                    break;
            }
            ChangeQuestCompletionStatus(ClickedCell);
        }

        /// <summary>
        /// Updates the selected character, then calls SelectedCharacterChanged
        /// to ensure items are updated appropriately.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void contextmenuCharSelect_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            ToolStripItem clickedItem = e.ClickedItem;
            SelectedCharacterName = clickedItem.Text;
            ChangeSelectedCharacter(SelectedCharacterName);
        }

        private void TcTabs_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tcTabs.TabPages[tcTabs.SelectedIndex].Text == "Sagas" && !SagasFormatted)
            {
                FormatSagaGrids();
                LoadSagaDataForCharacter();
            }
        }
    }

#nullable enable
    public class QuestPack
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Quest[] Quests { get; set; }
        public Wilderness[]? Wildernesses { get; set; }
    }

    public class Quest
    {
        public int Id { get; set; }
        public string Name { get; set; }
        /// <summary>
        /// WikiName is present if the wiki page is different from the quest name
        /// </summary>
        public string? WikiName { get; set; }
        public int? HeroicLevel { get; set; }
        public int? EpicLevel { get; set; }
        public int? LegLevel { get; set; }
        public string Patron { get; set; }
        public int Favor { get; set; }
        public string? Style { get; set; }
        /// <summary>
        /// SortWithPack is present if this quest should be grouped with a
        /// different pack from the one it is included in.
        /// This is mainly useful for F2P 'prologue' style quests.
        /// </summary>
        public string? SortWithPack { get; set; }
        // Style can be Solo, Raid, or null
    }

    /// <summary>
    /// A Wilderness object must have at least one
    /// level range, and should have a map picture name
    /// for each non-null level range.
    /// </summary>
    public class Wilderness
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? WikiName { get; set; }
        public string? HeroicLevelRange { get; set; }
        public string? EpicLevelRange { get; set; }
        public string? LegLevelRange { get; set; }
        public string? HeroicMap { get; set; }
        public string? EpicMap { get; set; }
        public string? LegMap { get; set; }

    }

    public class Character
    {
        public Dictionary<string, string> QuestCompletion { get; set; }
        public Dictionary<int, List<string>> SagaCompletion { get; set; }
        public Dictionary<string, string> PastLives { get; set; }
    }

    public class Saga
    {
        public string Id { get; set; }
        public string[] Name { get; set; }
        public string[] WikiName { get; set; }
        public int SortLevel { get; set; }
        public string[] NPC { get; set; }
        public int[] TomeLevel { get; set; }
        public string[] SpecialRewards { get; set; }
        public string[][] Quests { get; set; }
    }
}
