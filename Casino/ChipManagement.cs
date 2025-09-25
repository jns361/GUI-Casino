using System.IO;
using System.Text;

namespace Casino
{
    public class ChipManagement
    {
        public readonly GameLogicRoulette roulette;
        public RoulettePage ui;
        public int chipAmount;// = 100;



        //Set \Appdata\Roaming\Roulette path for savefile txt
        private readonly string saveDirectory = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "Roulette",
            "Savefile"
        );
        // Remove the duplicate and problematic field declaration
        // Replace:
        // public readonly string savePath = Path.Combine(path1: basePath, "Savefile.txt");

        // With a constructor assignment for savePath
        public string basePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Casino", "Savefile");
        public string savePath;

        public ChipManagement(GameLogicRoulette game, RoulettePage ui)
        {
            this.roulette = roulette;
            this.ui = ui;
            savePath = Path.Combine(basePath, "Savefile.txt");
            LoadChips();
        }

        //set amount after bet + amount after win
        public void SetChipAmount(bool gameWin, int betAmount, int PlayerNumber, string playerColor)
        {
            int prizeForWin = 0;
            //chipAmount -= betAmount;
            //ui.chipDisplay.Text = chipAmount.ToString();
            Animations.PopOut(ui.chipDisplay);
            //after color bet
            bool ColorGame = ui.GetBetType();
            if (gameWin)
            {
                if (ColorGame)
                {
                    if (playerColor.ToLower() == "red" || playerColor.ToLower() == "black")
                    {
                        prizeForWin = betAmount * 2;
                    }
                    else if (playerColor.ToLower() == "green")
                    {
                        prizeForWin = betAmount * 36;
                    }
                }
                else if (!ColorGame)
                {
                    prizeForWin = betAmount * 36;
                }
                chipAmount += prizeForWin;
                ui.chipDisplay.Text = chipAmount.ToString();
            }
            else if (!gameWin)
            {
                if (chipAmount == 0)
                {
                    ui.NoChipsLeft();
                }
                prizeForWin = 0;

                return;
            }
        }


        public void SaveChips()
        {
            savePath = Path.Combine(basePath, "Savefile.txt");
            try
            {
                // Korrekt: nur den Ordner erstellen
                Directory.CreateDirectory(Path.GetDirectoryName(savePath));

                using (StreamWriter sw = new StreamWriter(savePath, false, Encoding.ASCII))
                {
                    sw.Write(chipAmount);
                }
                Console.WriteLine("Save successful at: " + savePath);
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: " + e.Message);
            }
            finally
            {
                Console.WriteLine("SaveChips() finished for a chipAmount of " + chipAmount);
            }
        }

        public int ChipAmount => chipAmount;

        public void LoadChips()
        {
            try
            {
                savePath = Path.Combine(basePath, "Savefile.txt");
                if (File.Exists(savePath))
                {
                    string content = File.ReadAllText(savePath);
                    chipAmount = int.Parse(content);
                }
                /*else
                {
                    chipAmount = 1000;
                }*/
            }
            catch (Exception e)
            {
                Console.WriteLine("Error while loading chip amount: " + e.Message);
                chipAmount = 1000;
            }
        }
    }
}