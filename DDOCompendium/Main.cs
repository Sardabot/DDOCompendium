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
        public DataTable questsTable;
        public Character[] characterData;
        public int SelectedCharacter = Properties.Settings.Default.SelectedCharacter;
        public Main()
        {
            InitializeComponent();

            if (!ImportData()) Application.Exit();
        }

        private bool ImportData()
        {
            // import quest data
            if (Properties.Settings.Default.questsFilePath == "")
            {
                OpenFileDialog filedialog = new()
                {
                    Title = "Find Quests.json"
                };
                if (filedialog.ShowDialog() == DialogResult.OK)
                {
                    Properties.Settings.Default.questsFilePath = filedialog.FileName;
                }
                else return false;
            }
            using StreamReader reader = new(Properties.Settings.Default.questsFilePath);
            string importedJsonData = reader.ReadToEnd();
            reader.Close();
            var importedQuestData = JsonConvert.DeserializeObject<List<QuestPack>>(importedJsonData);
            if (importedQuestData is null)
            {
                MessageBox.Show("Couldn't retrieve data from Quests.json");
                return false;
            }
            // import user save data
            string CharactersFilePath = Properties.Settings.Default.questsFilePath.Replace("Quests.json", "Characters.json");
            using StreamReader reader2 = new(CharactersFilePath);
            importedJsonData = reader2.ReadToEnd();
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
            BindingSource questsDataSource = new()
            {
                DataSource = questsTable
            };
            datagridQuests.DataSource = questsDataSource;
            // hide the ID and Wiki Name columns
            datagridQuests.Columns[0].Visible = false;
            datagridQuests.Columns[1].Visible = false;

            return true;
        }

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

        private void ChangeSelectedCharacter()
        {

        }

        private string FindQuestCompletionStatus(string questID)
        {
            var allCompletion = characterData[SelectedCharacter].QuestCompletion;
            var thisCompletion = allCompletion.Where(n => n.Split(' ')[0] == questID).FirstOrDefault();
            if (thisCompletion == "" || thisCompletion is null) return "";
            switch (thisCompletion.Split(' ')[1])
            {
                case "N":
                    return "Normal";
                case "H":
                    return "Hard";
                case "E":
                    return "Elite";
                case "C":
                    return "Casual";
                default:
                    return "";
            }
        }

        private void DatagridQuests_CellClick(object sender, DataGridViewCellEventArgs e)
        {

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
        public string[] QuestCompletion { get; set; }
        public string[] PastLives { get; set; }
    }
}
