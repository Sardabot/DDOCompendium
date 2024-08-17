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
        public int SelectedCharacter = Properties.Settings.Default.SelectedCharacter;
        public string SelectedDifficulty = "Elite";
        public string LevelFilter = "All";
        private DataGridViewCell ClickedCell;
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
                    questsTable.Rows.Add(tempRow);
                }
            }
            questsDataView = new DataView(questsTable);
            BindingSource questsDataSource = new()
            {
                DataSource = questsDataView
            };
            datagridQuests.DataSource = questsDataSource;
            // hide the ID and Wiki Name columns
            datagridQuests.Columns[QUESTSGRID_ID_INDEX].Visible = false;
            datagridQuests.Columns[QUESTSGRID_WIKI_INDEX].Visible = false;
            datagridQuests.Columns[QUESTSGRID_EPIC_INDEX].SortMode = DataGridViewColumnSortMode.NotSortable;
            datagridQuests.Columns[QUESTSGRID_LEGENDARY_INDEX].SortMode = DataGridViewColumnSortMode.NotSortable;

            // import the notes tabs
            txtNotes1.Text = ReadFromFile(DataFolderPath + "Notes.txt");
            txtNotes2.Text = ReadFromFile(DataFolderPath + "Notes2.txt");

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

            return table;
        }

        private void SelectedCharacterChanged()
        {
            foreach (DataGridViewRow thisRow in datagridQuests.Rows)
            {
                thisRow.Cells[QUESTSGRID_COMPLETED_INDEX].Value = FindQuestCompletionStatus(thisRow.Cells[QUESTSGRID_ID_INDEX].Value.ToString());
            }
        }

        private string FindQuestCompletionStatus(string questID)
        {
            var allCompletion = characterData[SelectedCharacter].QuestCompletion;
            var thisCompletion = allCompletion.Where(n => n.Split(' ')[0] == questID).FirstOrDefault();
            if (thisCompletion == "" || thisCompletion is null) return "";
            else return thisCompletion.Split(' ')[1];
        }

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
            foreach (string thisval in characterData[SelectedCharacter].QuestCompletion)
            {
                if (thisval.Split(' ')[0] == QuestID)
                {
                    valindex = characterData[SelectedCharacter].QuestCompletion.IndexOf(thisval);
                    foundval = thisval;
                    break;
                }
            }
            string newDiff = thisCell.Value.ToString();
            if (newDiff == "")
            {
                if (valindex != -1) characterData[SelectedCharacter].QuestCompletion.Remove(foundval);
            }
            else
            {
                if (valindex == -1) characterData[SelectedCharacter].QuestCompletion.Add(QuestID + " " + thisCell.Value.ToString());
                else characterData[SelectedCharacter].QuestCompletion[valindex] = QuestID + " " + thisCell.Value.ToString();
            }
        }

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
                        switch (LevelFilter)
                        {
                            case "All":
                            case "Legendary":
                                LevelFilter = "Epic";
                                questsDataView.RowFilter = "E > 19 AND L > 29";
                                break;
                            case "Epic":
                                LevelFilter = "All";
                                questsDataView.RowFilter = "";
                                break;
                        }
                        break;
                    case QUESTSGRID_LEGENDARY_INDEX:
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

        private void DatagridQuests_CellContextMenuStripNeeded(object sender, DataGridViewCellContextMenuStripNeededEventArgs e)
        {
            if (e.ColumnIndex == QUESTSGRID_COMPLETED_INDEX && e.RowIndex != -1)
            {
                e.ContextMenuStrip = contextmenuQuestCompletion;
                ClickedCell = datagridQuests.Rows[e.RowIndex].Cells[e.ColumnIndex];
            }
            else if (e.ColumnIndex == QUESTSGRID_COMPLETED_INDEX && e.RowIndex == -1)
            {
                e.ContextMenuStrip = contextmenuCharSelect;
                contextmenuCharSelect.Items.Clear();
                foreach (string thisCharName in characterData.Select(c => c.Name))
                {
                    contextmenuCharSelect.Items.Add(thisCharName);
                }
            }
        }

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

        private void contextmenuCharSelect_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            ToolStripItem clickedItem = e.ClickedItem;
            SelectedCharacter = characterData.Where(item => item.Name == clickedItem.Text).FirstOrDefault().Id;
            SelectedCharacterChanged();
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
        public string? WikiName { get; set; }
        // WikiName is present if the wiki page is different from the quest name
        public int? HeroicLevel { get; set; }
        public int? EpicLevel { get; set; }
        public int? LegLevel { get; set; }
        public string Patron { get; set; }
        public int Favor { get; set; }
        public string? Style { get; set; }
        // Style can be Solo, Raid, or null
    }

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
        public List<string> PastLives { get; set; }
    }
}
