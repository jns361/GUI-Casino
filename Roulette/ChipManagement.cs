using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Security.AccessControl;
using System.Security.Cryptography;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;

namespace Roulette
{
    public class ChipManagement
    {        
        public readonly GameLogic game;
        public readonly MainWindow interactions;

        public int chipAmount;// = 100;
        
        //Set \Appdata\Roaming\Roulette path for savefile txt
        private readonly string saveDirectory = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "Roulette",
            "Savefile"
        );
        private readonly string cdEncrypt = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "Roulette",
            "Savefile",
            "Savefile.txt"
        );

        private readonly string savePath;

        public ChipManagement(GameLogic game, MainWindow interactions)
        {
            this.game = game;
            this.interactions = interactions;

            savePath = Path.Combine(saveDirectory, "Savefile.txt");

            LoadChips();            
        }
        
        //set amount after bet + amount after win
        public void SetChipAmount(bool gameWin, int betAmount, int PlayerNumber, string playerColor)               
        {
            chipAmount -= betAmount;
            interactions.chipDisplay.Text=chipAmount.ToString();
            Animations.PopOut(interactions.chipDisplay);
            //after color bet
            bool ColorGame = interactions.GetBetType();
            int prizeForWin = 0;
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
                interactions.chipDisplay.Text= chipAmount.ToString();
            }
            else if (!gameWin)
            {
                prizeForWin = 0;
            }
        }

        public int GetBetAmount(int betAmount)
        {
            return betAmount;
        }

        public void SaveChips()
        {
            try
            {                
                //Create folder, if not existing already
                Directory.CreateDirectory(saveDirectory);

                //Set path to file
                //string filePath = Path.Combine(savePath, "Savefile.txt");

                //Open, write and close
                using (StreamWriter sw = new StreamWriter(savePath, false, Encoding.ASCII))
                {
                    sw.Write(chipAmount);
                }
                Console.WriteLine("Save successful at: " + savePath);
            }
            catch(Exception e)
            {
                Console.WriteLine("Exception: " + e.Message);
            }
            finally
            {
                Console.WriteLine("SaveChips() finished");
            }
        }

        public int GetChipAmount() 
        { 
            return chipAmount; 
        }

        public void LoadChips()
        {   
            try
            {
               // File.Decrypt = @"cdEncrypt";
                if (File.Exists(savePath))
                {
                    string content = File.ReadAllText(savePath);
                    chipAmount = int.Parse(content);
                }
                else
                {
                    chipAmount = 1000;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error while loading chip amount: " + e.Message);
                chipAmount = 1000;
            }
        }
    }
}