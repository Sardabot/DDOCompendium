using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
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
        public Dictionary<string, List<int?>> PackSortLevels = [];
        public bool SagasFormatted = false;
        public string SelectedCharacterName = Properties.Settings.Default.SelectedCharacterName;
        public string SelectedDifficulty = "Elite";
        /// <summary>
        /// Can be All, Epic, or Legendary
        /// </summary>
        public string LevelFilter = "All";
        /// <summary>
        /// Can be None, Pack, Level, Patron
        /// </summary>
        public string CurrentSort = "None";
        private DataGridViewCell ClickedCell;
        private int ClickedSagaID;
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
            var importedQuestData = JsonConvert.DeserializeObject<Dictionary<string, QuestPack>>(importedJsonData);
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
            foreach ((string thisPackName, QuestPack thisPack) in importedQuestData)
            //foreach (string thisPackName in importedQuestData.Keys)
            {
                PackSortLevels.Add(thisPackName, importedQuestData[thisPackName].SortLevels);

                foreach ((string thisQuestName, Quest thisQuest) in thisPack.Quests)
                //foreach (string thisQuestName in importedQuestData[thisPackName].Quests.Keys)
                {
                    DataRow tempRow = questsTable.NewRow();
                    tempRow.SetField("H", thisQuest.HeroicLevel);
                    tempRow.SetField("E", thisQuest.EpicLevel);
                    tempRow.SetField("L", thisQuest.LegLevel);
                    tempRow.SetField("Name", thisQuestName);
                    tempRow.SetField("Pack", thisPackName);
                    tempRow.SetField("Character", "");
                    tempRow.SetField("Patron", thisQuest.Patron);
                    tempRow.SetField("Favor", thisQuest.Favor);
                    tempRow.SetField("Style", thisQuest.Style);
                    var sortpack = thisQuest.SortWithPack;
                    if (sortpack == null) sortpack = thisPackName;
                    else UpdatePackSortLevels(sortpack, thisQuest.HeroicLevel, thisQuest.EpicLevel, thisQuest.LegLevel);
                    tempRow.SetField("SortWithPack", sortpack);
                    tempRow.SetField("WikiName", thisQuest.WikiName);
                    questsTable.Rows.Add(tempRow);
                }
            }
            FinalizePackSortLevels();
            questsDataView = new DataView(questsTable);
            questsDataView.Sort = "SortExpr ASC";
            BindingSource questsDataSource = new()
            {
                DataSource = questsDataView
            };
            datagridQuests.DataSource = questsDataSource;
            // make adjustments to grid settings
            foreach (DataGridViewColumn column in datagridQuests.Columns) column.SortMode = DataGridViewColumnSortMode.Programmatic;
            datagridQuests.Columns[QUESTSGRID_COMPLETED_INDEX].HeaderText = SelectedCharacterName;
            datagridQuests.Columns[QUESTSGRID_WIKI_INDEX].Visible = false;

            // import the notes tabs
            txtNotes1.Text = ReadFromFile(DataFolderPath + "Notes.txt");
            txtNotes2.Text = ReadFromFile(DataFolderPath + "Notes2.txt");

            // import ref tables

            // import wilderness tables

            // setup characters tab
            foreach (string charname in characterData.Keys) cmboCharSelect.Items.Add(charname);
            cmboCharSelect.SelectedItem = SelectedCharacterName;

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
                thisdgview.CellContextMenuStripNeeded += DatagridSagas_CellContextMenuStripNeeded;
                tableLayoutPanelSagas.RowCount += 1;
                tableLayoutPanelSagas.Controls.Add(thisdgview, 0, tableLayoutPanelSagas.RowCount - 1);
            }

            // setup settings tab

            // load character data into things
            ChangeSelectedCharacter(SelectedCharacterName);

            return true;
        }

        private void UpdatePackSortLevels(string sortpack, int? heroicLevel, int? epicLevel, int? legLevel)
        {
            if (PackSortLevels.TryGetValue(sortpack, out List<int?> foundLevels))
            {
                if (heroicLevel.HasValue)
                {
                    foundLevels[0] = foundLevels[0].HasValue ? Math.Min(foundLevels[0].Value, heroicLevel.Value) : heroicLevel.Value;
                }
                if (epicLevel.HasValue)
                {
                    foundLevels[1] = foundLevels[1].HasValue ? Math.Min(foundLevels[1].Value, epicLevel.Value) : epicLevel.Value;
                }
                if (legLevel.HasValue)
                {
                    foundLevels[2] = foundLevels[2].HasValue ? Math.Min(foundLevels[2].Value, legLevel.Value) : legLevel.Value;
                }
                PackSortLevels[sortpack] = foundLevels;
            }
            else
            {
                PackSortLevels.Add(sortpack, [ heroicLevel, epicLevel, legLevel ]);
            }
        }

        private void FinalizePackSortLevels()
        {
            var tempcopy = new List<string>(PackSortLevels.Keys);
            foreach (string packName in tempcopy)
            {
                int? heroicLevel = PackSortLevels[packName][0];
                int? epicLevel = PackSortLevels[packName][1];
                int? legLevel = PackSortLevels[packName][2];


                // at least one level should have value but do this just in case
                if (!heroicLevel.HasValue && !epicLevel.HasValue && !legLevel.HasValue)
                {
                    heroicLevel = 1;
                    epicLevel = 1;
                    legLevel = 1;
                }
                // if no heroic level, replace with epic or leg level in that order
                heroicLevel ??= epicLevel ?? legLevel;
                // if no epic level, replace with leg or heroic level in that order
                epicLevel ??= legLevel ?? heroicLevel;
                // if no leg level, replace with epic level
                legLevel ??= epicLevel;

                PackSortLevels[packName] = [heroicLevel, epicLevel, legLevel];
            }
        }

        public const int QUESTSGRID_HEROIC_INDEX = 0;
        public const int QUESTSGRID_EPIC_INDEX = 1;
        public const int QUESTSGRID_LEGENDARY_INDEX = 2;
        public const int QUESTSGRID_NAME_INDEX = 3;
        public const int QUESTSGRID_PACK_INDEX = 4;
        public const int QUESTSGRID_COMPLETED_INDEX = 5;
        public const int QUESTSGRID_PATRON_INDEX = 6;
        public const int QUESTSGRID_FAVOR_INDEX = 7;
        public const int QUESTSGRID_STYLE_INDEX = 8;
        public const int QUESTSGRID_PACKSORT_INDEX = 9;
        public const int QUESTSGRID_WIKI_INDEX = 10;
        public const int QUESTSGRID_SORT_EXPR_INDEX = 11;

        private DataTable MakeQuestsTable()
        {
            DataTable table = new()
            {
                TableName = "Quests"
            };
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
            table.Columns.Add(new DataColumn("WikiName", typeof(string)));
            table.Columns.Add(new DataColumn("SortExpr", typeof(string)));

            return table;
        }

        private DataTable MakeSagaTable(string[][] dataIn)
        {
            DataTable table = new();
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
            LoadPastLivesAndTomesForCharacter();
            datagridQuests.Columns[QUESTSGRID_COMPLETED_INDEX].HeaderText = SelectedCharacterName;
            Text = "DDO Compendium - " + SelectedCharacterName;
        }

        private void LoadQuestDataForCharacter()
        {
            foreach (DataRow thisRow in questsTable.Rows)
            {
                thisRow["Character"] = FindQuestCompletionStatus(thisRow["Name"].ToString());
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
                    charThisSagaData ??= [];
                    for (int row = 0; row < thisSagadg.RowCount; row++)
                    {
                        if (row < charThisSagaData.Count) thisSagadg.Rows[row].Cells[2 + i].Value = charThisSagaData[row];
                        else thisSagadg.Rows[row].Cells[2 + i].Value = "";
                    }
                }
            }
        }

        private void LoadPastLivesAndTomesForCharacter()
        {
            string tomeval;
            foreach (NumericUpDown thisnumbox in splitContainerCharacters.Panel1.Controls.OfType<NumericUpDown>())
            {
                var tomename = thisnumbox.Tag as string;
                if (characterData[SelectedCharacterName].Tomes.TryGetValue(tomename, out tomeval))
                {
                    thisnumbox.Value = int.Parse(tomeval);
                }
                else thisnumbox.Value = 0;
            }
            if (characterData[SelectedCharacterName].Tomes.TryGetValue("Heroic Learning", out tomeval))
            {
                cmboHeroicLearning.Text = tomeval;
            }
            else cmboHeroicLearning.Text = "";
            if (characterData[SelectedCharacterName].Tomes.TryGetValue("Epic Learning", out tomeval))
            {
                cmboEpicLearning.Text = tomeval;
            }
            else cmboEpicLearning.Text = "";
            foreach (NumericUpDown thisnumbox in splitContainerCharacters.Panel2.Controls.OfType<NumericUpDown>())
            {
                var lifename = thisnumbox.Tag as string;
                if (characterData[SelectedCharacterName].PastLives.TryGetValue(lifename, out string lifeval))
                {
                    thisnumbox.Value = int.Parse(lifeval);
                }
                else thisnumbox.Value = 0;
            }
        }

        /// <summary>
        /// Searches characterData for the completion status of a quest.
        /// Uses the currently selected character.
        /// </summary>
        /// <param name="questID">The ID to search for, ex. "1-4"</param>
        /// <returns>A string representing the quest difficulty.</returns>
        private string FindQuestCompletionStatus(string questName)
        {
            return characterData[SelectedCharacterName].QuestCompletion.TryGetValue(questName, out var completion) ? completion : "";
        }

        /// <summary>
        /// Updates the text in the Quests datagridview completion column for a quest.
        /// Also updates the associated entry in the characterData object.
        /// </summary>
        /// <param name="thisCell">The affected cell.</param>
        private void ChangeQuestCompletionStatus(DataGridViewCell thisCell, string chosenDiff)
        {
            string QuestName = datagridQuests.Rows[thisCell.RowIndex].Cells[QUESTSGRID_NAME_INDEX].Value.ToString();

            if (thisCell.Value.ToString() == chosenDiff) thisCell.Value = "";
            else if (datagridQuests.Rows[thisCell.RowIndex].Cells[QUESTSGRID_STYLE_INDEX].Value.ToString() == "Solo") thisCell.Value = "Casual";
            else thisCell.Value = chosenDiff;

            if (thisCell.Value.ToString() == "")
            {
                characterData[SelectedCharacterName].QuestCompletion.Remove(QuestName);
            }
            else
            {
                if (characterData[SelectedCharacterName].QuestCompletion.ContainsKey(QuestName))
                {
                    characterData[SelectedCharacterName].QuestCompletion[QuestName] = thisCell.Value.ToString();
                }
                else characterData[SelectedCharacterName].QuestCompletion.Add(QuestName, thisCell.Value.ToString());
            }
        }

        private void ChangeSagaQuestCompletionStatus(int sagaID, DataGridViewCell thisCell, string chosenDiff)
        {
            if (thisCell.Value.ToString() == chosenDiff) thisCell.Value = "";
            else thisCell.Value = chosenDiff;

            if (!characterData[SelectedCharacterName].SagaCompletion.ContainsKey(sagaID))
            {
                characterData[SelectedCharacterName].SagaCompletion.Add(sagaID, []);
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
                            ChangeQuestCompletionStatus(datagridQuests.Rows[thisRowIndex].Cells[thisColumnIndex], SelectedDifficulty);
                        }
                        break;
                    case QUESTSGRID_EPIC_INDEX:
                        if (thisRowIndex == -1)
                        {    
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
                            RefreshSortOrder();
                        }
                        break;
                    case QUESTSGRID_LEGENDARY_INDEX:
                        if (thisRowIndex == -1)
                        {
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
                            RefreshSortOrder();
                        }
                        break;
                    case QUESTSGRID_PACK_INDEX:
                        if (thisRowIndex == -1)
                        {
                            // sort by pack with respect to pack level
                            SortQuestsByPack();
                        }
                        break;
                    default:
                        break;
                }
            }
        }

        private void SortQuestsByPack()
        {
            foreach (DataRow thisQuest in questsTable.Rows)
            {
                int filterindex = 0;
                if (LevelFilter == "Epic") filterindex = 1;
                else if (LevelFilter == "Legendary") filterindex = 2;
                string sortpack = thisQuest["SortWithPack"].ToString();
                string sortlevel = "";
                if (!cbxKeepFreeTogether.Checked && sortpack == "Free")
                {
                    if (filterindex == 0) sortlevel = thisQuest["H"].ToString();
                    if (filterindex == 1 || sortlevel == "") sortlevel = thisQuest["E"].ToString();
                    if (filterindex == 2 || sortlevel == "") sortlevel = thisQuest["L"].ToString();
                }
                else sortlevel = PackSortLevels[sortpack][filterindex].ToString();
                sortlevel = sortlevel.PadLeft(3, '0');
                thisQuest["SortExpr"] = sortlevel + " " + sortpack;
            }
            CurrentSort = "Pack";
        }

        private void RefreshSortOrder()
        {
            switch (CurrentSort)
            {
                case "Pack":
                    {
                        SortQuestsByPack();
                        break;
                    }
                default: break;
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
                            ChangeSagaQuestCompletionStatus(sagaID, thisdgview.Rows[thisRowIndex].Cells[thisColumnIndex], SelectedDifficulty);
                        }
                        break;
                }
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
            using StreamReader reader = new(filePath);
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
        /// Provides the appropriate context menu based on which column
        /// of the quests datagridview was right-clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DatagridSagas_CellContextMenuStripNeeded(object sender, DataGridViewCellContextMenuStripNeededEventArgs e)
        {
            var thisdgv = sender as DataGridView;
            if (e.ColumnIndex > 1 && e.RowIndex != -1)
            {
                // right-clicked a non-header cell in any completion status column
                // we want to provide options to change the selected difficulty
                e.ContextMenuStrip = contextmenuSagaCompletion;
                ClickedCell = thisdgv.Rows[e.RowIndex].Cells[e.ColumnIndex];
                ClickedSagaID = int.Parse((thisdgv.Tag as string).Split(',')[ClickedCell.ColumnIndex - 2]);
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
            string thisDiff;
            switch (clickedItem.Text)
            {
                case "Clear":
                    thisDiff = "";
                    break;
                case "Casual":
                case "Normal":
                case "Hard":
                case "Elite":
                    SelectedDifficulty = clickedItem.Text;
                    thisDiff = SelectedDifficulty;
                    break;
                default:
                    thisDiff = "";
                    break;
            }
            ChangeQuestCompletionStatus(ClickedCell, thisDiff);
        }

        private void ContextmenuSagaCompletion_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            ToolStripItem clickedItem = e.ClickedItem;
            string thisDiff;
            switch (clickedItem.Text)
            {
                case "Clear":
                    thisDiff = "";
                    break;
                case "Casual":
                case "Normal":
                case "Hard":
                case "Elite":
                    SelectedDifficulty = clickedItem.Text;
                    thisDiff = SelectedDifficulty;
                    break;
                default:
                    thisDiff = "";
                    break;
            }
            ChangeSagaQuestCompletionStatus(ClickedSagaID, ClickedCell, thisDiff);
        }

        /// <summary>
        /// Updates the selected character, then calls SelectedCharacterChanged
        /// to ensure items are updated appropriately.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ContextmenuCharSelect_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
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

        private void NumTomes_ValueChanged(object sender, EventArgs e)
        {
            var thisNumBox = sender as NumericUpDown;
            var tomename = thisNumBox.Tag as string;
            if (characterData[SelectedCharacterName].Tomes.TryGetValue(tomename, out string _))
            {
                characterData[SelectedCharacterName].Tomes[tomename] = thisNumBox.Value.ToString();
            }
            else characterData[SelectedCharacterName].Tomes.Add(tomename, thisNumBox.Value.ToString());
        }

        private void NumPastLives_ValueChanged(object sender, EventArgs e)
        {
            var thisNumBox = sender as NumericUpDown;
            var pastlifename = thisNumBox.Tag as string;
            if (characterData[SelectedCharacterName].PastLives.TryGetValue(pastlifename, out string _))
            {
                characterData[SelectedCharacterName].PastLives[pastlifename] = thisNumBox.Value.ToString();
            }
            else characterData[SelectedCharacterName].PastLives.Add(pastlifename, thisNumBox.Value.ToString());
        }

        private void CmboHeroicLearning_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (characterData[SelectedCharacterName].Tomes.TryGetValue("Heroic Learning", out string _))
            {
                characterData[SelectedCharacterName].Tomes["Heroic Learning"] = cmboHeroicLearning.SelectedItem.ToString();
            }
            else characterData[SelectedCharacterName].Tomes.Add("Heroic Learning", cmboHeroicLearning.SelectedItem.ToString());
        }

        private void CmboEpicLearning_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (characterData[SelectedCharacterName].Tomes.TryGetValue("Epic Learning", out string _))
            {
                characterData[SelectedCharacterName].Tomes["Epic Learning"] = cmboEpicLearning.SelectedItem.ToString();
            }
            else characterData[SelectedCharacterName].Tomes.Add("Epic Learning", cmboEpicLearning.SelectedItem.ToString());
        }

        private void CmboCharSelect_SelectedIndexChanged(object sender, EventArgs e)
        {
            ChangeSelectedCharacter(cmboCharSelect.SelectedItem.ToString());
        }

        private void BtnAddChar_Click(object sender, EventArgs e)
        {
            string newcharname = Prompt.ShowDialog("New character name:", "Add New Character");
            Character newchar = new()
            {
                QuestCompletion = [],
                SagaCompletion = [],
                PastLives = [],
                Tomes = []
            };
            characterData.Add(newcharname, newchar);
            cmboCharSelect.Items.Add(newcharname);
            cmboCharSelect.Text = newcharname;
        }

        private void BtnDelChar_Click(object sender, EventArgs e)
        {
            var response = MessageBox.Show("Are you sure you want to delete " + SelectedCharacterName + "?  This is irreversible!", "Delete character?", MessageBoxButtons.OKCancel);
            if (response == DialogResult.OK)
            {
                characterData.Remove(SelectedCharacterName);
                cmboCharSelect.Items.Remove(SelectedCharacterName);
                cmboCharSelect.SelectedIndex = 0;
            }
        }
    }

#nullable enable
    public class QuestPack
    {
        /// <summary>
        /// Should be an list of 3 ints representing Heroic, Epic, and Legendary levels
        /// to use when sorting by pack.
        /// Sorting by pack will normally take the first value, but if the table is filtered
        /// to Epic+Leg or Leg quests it will use the second or third value respectively.
        /// As such, a pack with no heroic quests should copy down the epic value, (or leg value
        /// if it has no epic quests either).
        /// A pack with no epic quests should copy down the leg value.
        /// A pack with no legendary quests, or only heroic quests, should copy the highest
        /// value up.  This value won't be used (as the quests will be filtered out anyways)
        /// but must still be present for code reasons.
        /// </summary>
        public List<int?> SortLevels { get; set; }
        public Dictionary<string, Quest> Quests { get; set; }
        public Wilderness[]? Wildernesses { get; set; }
    }

    public class Quest
    {
        /// <summary>
        /// WikiName is present if the wiki page is different from the quest name.
        /// This is what will be used by the url generator instead of the quest name
        /// in that case.
        /// </summary>
        public string? WikiName { get; set; }
        public int? HeroicLevel { get; set; }
        public int? EpicLevel { get; set; }
        public int? LegLevel { get; set; }
        public string Patron { get; set; }
        public int Favor { get; set; }
        /// <summary>
        /// Style can be Solo or Raid where applicable, and null otherwise
        /// </summary>
        public string? Style { get; set; }
        /// <summary>
        /// SortWithPack is present if this quest should be grouped with a
        /// different pack from the one it is included in.
        /// This is mainly useful for F2P 'prologue' style quests.
        /// Doesn't have to be a real pack - ex. to group together the Waterworks quests.
        /// Note that fake packs will have their sort levels dynamically generated based on
        /// lowest level (per level range) of all quests assigned to the pack.
        /// </summary>
        public string? SortWithPack { get; set; }
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
        public Dictionary<string, string> Tomes { get; set; }
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

    public static class Prompt
    {
        public static string ShowDialog(string text, string caption)
        {
            Form prompt = new()
            {
                Width = 500,
                Height = 150,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                Text = caption,
                StartPosition = FormStartPosition.CenterParent
            };
            Label textLabel = new() { Left = 50, Top = 20, Text = text };
            TextBox textBox = new() { Left = 50, Top = 50, Width = 400 };
            Button confirmation = new() { Text = "Ok", Left = 350, Width = 100, Top = 70, DialogResult = DialogResult.OK };
            confirmation.Click += (sender, e) => { prompt.Close(); };
            prompt.Controls.Add(textBox);
            prompt.Controls.Add(confirmation);
            prompt.Controls.Add(textLabel);
            prompt.AcceptButton = confirmation;

            return prompt.ShowDialog() == DialogResult.OK ? textBox.Text : "";
        }
    }

    public static class KeyValuePairExtensions
    {
        public static void Deconstruct<Tkey, TValue>(this KeyValuePair<Tkey, TValue> keyValuePair, out Tkey key, out TValue value)
        {
            key = keyValuePair.Key;
            value = keyValuePair.Value;
        }
    }
}
