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
        public Character[] characterData;
        public List<DataTable> SagaTables = [];
        public SagaData sagaData;
        public int SelectedCharacterID = Properties.Settings.Default.SelectedCharacter;
        public string SelectedCharacterName = Properties.Settings.Default.SelectedCharacterName;
        public string SelectedDifficulty = "Elite";
        public string LevelFilter = "All";
        private DataGridViewCell ClickedCell;
        public DataGridViewCellStyle darkgridcellstyle = new()
        {
            BackColor = Color.FromArgb(64, 64, 64),
            ForeColor = Color.FromArgb(224, 224, 244)
        };

        public Main()
        {
            InitializeComponent();

            if (!ImportData()) Application.Exit();
        }

        private bool ImportData()
        {
            // import quest data
            //if (Properties.Settings.Default.questsFilePath == "")
            //{
            //    OpenFileDialog filedialog = new()
            //    {
            //        Title = "Find Quests.json"
            //    };
            //    if (filedialog.ShowDialog() == DialogResult.OK)
            //    {
            //        Properties.Settings.Default.questsFilePath = filedialog.FileName;
            //    }
            //    else return false;
            //}
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
            var importedCharData = JsonConvert.DeserializeObject<List<Character>>(importedJsonData);
            if (importedCharData is null)
            {
                MessageBox.Show("Couldn't retrieve data from Characters.json");
                return false;
            }
            characterData = importedCharData.ToArray();

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
                    tempRow.SetField("Character", FindQuestCompletionStatus(tempID));
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
            sagaData = JsonConvert.DeserializeObject<SagaData>(importedJsonData);
            foreach (Saga thisSagaData in sagaData.SagaInfos)
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
                };
                tableLayoutPanelSagas.RowCount += 1;
                tableLayoutPanelSagas.Controls.Add(thisdgview, 0, tableLayoutPanelSagas.RowCount - 1);
            }

            // setup settings tab

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

        private void ResizeSagaGrids()
        {
            foreach (DataGridView thisdgview in tableLayoutPanelSagas.Controls)
            {
                thisdgview.Height = thisdgview.ColumnHeadersHeight + thisdgview.Rows.Cast<DataGridViewRow>().Sum(r => r.Height);
            }
        }

        /// <summary>
        /// Updates the quests table with the completion information for the
        /// newly selected character.  Also updates the column header text to the
        /// name of the newly selected character.
        /// </summary>
        private void SelectedCharacterChanged()
        {
            foreach (DataRow thisRow in questsTable.Rows)
            {
                thisRow["Character"] = FindQuestCompletionStatus(thisRow["ID"].ToString());
            }
            datagridQuests.Columns[QUESTSGRID_COMPLETED_INDEX].HeaderText = SelectedCharacterName;
        }

        /// <summary>
        /// Searches characterData for the completion status of a quest.
        /// Uses the currently selected character.
        /// </summary>
        /// <param name="questID">The ID to search for, ex. "1-4"</param>
        /// <returns>A string representing the quest difficulty.</returns>
        private string FindQuestCompletionStatus(string questID)
        {
            var allCompletion = characterData[SelectedCharacterID].QuestCompletion;
            var thisCompletion = allCompletion.Where(n => n.Split(' ')[0] == questID).FirstOrDefault();
            if (thisCompletion is null || thisCompletion == "") return "";
            else return thisCompletion.Split(' ')[1];
        }

        /// <summary>
        /// Updates the text in the Quests datagridview completion column for a quest.
        /// Also updates the associated entry in the characterData object.
        /// </summary>
        /// <param name="thisCell">The affected cell.</param>
        private void ChangeQuestCompletionStatus(DataGridViewCell thisCell)
        {
            string QuestID = datagridQuests.Rows[thisCell.RowIndex].Cells[QUESTSGRID_ID_INDEX].Value.ToString();
            switch (thisCell.Value.ToString())
            {
                case "Casual":
                case "Normal":
                case "Hard":
                case "Elite":
                    if (thisCell.Value.ToString() == SelectedDifficulty) thisCell.Value = "";
                    else thisCell.Value = SelectedDifficulty;
                    break;
                default:
                    if (datagridQuests.Rows[thisCell.RowIndex].Cells[QUESTSGRID_STYLE_INDEX].Value.ToString() == "Solo")
                    {
                        thisCell.Value = "Casual";
                    }
                    else thisCell.Value = SelectedDifficulty;
                    break;
            }
            int valindex = -1;
            string foundval = "";
            foreach (string thisval in characterData[SelectedCharacterID].QuestCompletion)
            {
                if (thisval.Split(' ')[0] == QuestID)
                {
                    valindex = characterData[SelectedCharacterID].QuestCompletion.IndexOf(thisval);
                    foundval = thisval;
                    break;
                }
            }
            string newDiff = thisCell.Value.ToString();
            if (newDiff == "")
            {
                if (valindex != -1) characterData[SelectedCharacterID].QuestCompletion.Remove(foundval);
            }
            else
            {
                if (valindex == -1) characterData[SelectedCharacterID].QuestCompletion.Add(QuestID + " " + thisCell.Value.ToString());
                else characterData[SelectedCharacterID].QuestCompletion[valindex] = QuestID + " " + thisCell.Value.ToString();
            }
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
            else if (e.Button == MouseButtons.Right)
            {

            }
        }

        private void Main_FormClosing(object sender, FormClosingEventArgs e)
        {
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
                foreach (string thisCharName in characterData.Select(c => c.Name))
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
            SelectedCharacterID = characterData.Where(item => item.Name == clickedItem.Text).FirstOrDefault().Id;
            SelectedCharacterChanged();
        }

        private void TcTabs_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tcTabs.TabPages[tcTabs.SelectedIndex].Text == "Sagas")
            {
                ResizeSagaGrids();
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
        public int Id { get; set; }
        public string Name { get; set; }
        public List<string> QuestCompletion { get; set; }
        public List<string> SagaCompletion { get; set; }
        public List<string> PastLives { get; set; }
    }

    public class SagaData
    {
        public Saga[] SagaInfos { get; set; }
        public string[][][] SagaTables { get; set; }
    }
    public class Saga
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int SortLevel { get; set; }
        public string NPC { get; set; }
        public int TomeLevel { get; set; }
        public string SpecialRewards { get; set; }
        public string[][] Quests { get; set; }
    }
}
