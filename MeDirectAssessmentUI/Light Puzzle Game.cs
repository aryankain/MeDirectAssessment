using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Windows.Forms;

namespace MeDirectAssessmentUI
{
    public partial class Form1 : Form
    {
        public Button[,] lights;
        public bool[,] lightsMatrix;
        IDictionary<string, string> appSettings = new Dictionary<string, string>();
        string URI = "http://localhost:51448/api/game";
        public MessageBoxButtons MB_OK { get; private set; }

        public Form1()
        {
            GetAppSettings(URI);
            InitializeComponent();
        }
        private async void GetAppSettings(string url)
        {
            if (panel1 != null)
            {
                panel1.Controls.OfType<Button>().ToList().ForEach(btn => btn.Dispose());
            }
            try
            {
                using (var client = new HttpClient())
                {
                    using (var response = await client.GetAsync(url))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            var appSettingsString = await response.Content.ReadAsStringAsync();

                            appSettings = JsonConvert.DeserializeObject<Dictionary<string, string>>(appSettingsString);

                            if (Convert.ToBoolean(appSettings["Intialize"]))
                            {
                                lights = new Button[Convert.ToInt32(appSettings["Grid"]), Convert.ToInt32(appSettings["Grid"])];
                                lightsMatrix = new bool[Convert.ToInt32(appSettings["Grid"]), Convert.ToInt32(appSettings["Grid"])];
                                randomStart();
                            }
                            else
                            {
                                MessageBox.Show("No Permission to start the game.");
                            }
                        }
                        else
                        {
                            MessageBox.Show("Sevice is not running.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Sevice is not running.");
            }

        }

        public void randomStart()
        {
            // Loop through all lights to generate the buttons on form
            for (int i = 0; i < lights.GetLength(1); i++)
            {
                for (int j = 0; j < lights.GetLength(0); j++)
                {
                    lights[i, j] = new Button();
                    lights[i, j].Size = new Size(40, 40);
                    // Name of button object is index in 3D array
                    lights[i, j].Name = i.ToString() + j.ToString();
                    lights[i, j].Click += light_Click;
                    lights[i, j].Location = new Point(30 + (j * 60), 20 + (i * 60));
                    lights[i, j].BackColor = Color.Black;
                    lightsMatrix[i, j] = false;
                    panel1.Controls.Add(lights[i, j]);
                }
            }

            Random rnd = new Random();
            // Random number of lights to turn on between 1 and 10
            for (int i = 0; i < rnd.Next(1, 10); i++)
            {
                // Get random indexes
                int x = rnd.Next(0, lights.GetLength(1));
                int y = rnd.Next(0, lights.GetLength(0));
                // Call invertButton function with indexes
                invertButton(lights[x, y], x, y);
            }

            // Special case check
            // If lights have been toggled and no lights remain on
            // Set one random light to be on
            if (checkStatus() == true)
            {
                // Get random indexes
                int x = rnd.Next(0, lights.GetLength(1));
                int y = rnd.Next(0, lights.GetLength(0));
                // Call invertButton function with indexes
                invertButton(lights[x, y], x, y);
            }
        }

        public void light_Click(object sender, EventArgs e)
        {
            Button b = sender as Button; //Clicked object is a Button
            // Get the index of the button clicked
            int i = (int)Char.GetNumericValue(b.Name[0]);
            int j = (int)Char.GetNumericValue(b.Name[1]);

            // Invert button clicked
            invertHandler(lights[i, j], i, j);

            // Check if game has ended
            checkEnd();
        }

        public void invertHandler(object sender, int i, int j)
        {
            // Invert button clicked
            invertButton(lights[i, j], i, j);

            // Invert the correct buttons around the clicked one
            if (i > 0)
            {
                invertButton(lights[i - 1, j], i - 1, j); //Above
            }
            if (i < (lights.GetLength(1) - 1))
            {
                invertButton(lights[i + 1, j], i + 1, j); //Below
            }
            if (j > 0)
            {
                invertButton(lights[i, j - 1], i, j - 1); //Left
            }
            if (j < (lights.GetLength(1) - 1))
            {
                invertButton(lights[i, j + 1], i, j + 1); //Right
            }

        }

        public void invertButton(object sender, int i, int j)
        {
            Button b = sender as Button; //Clicked object is a Button

            // Invert the boolean value for the button
            lightsMatrix[i, j] = !lightsMatrix[i, j];

            // If light is turned on
            if (lightsMatrix[i, j] == true)
            {
                // Set colour of matched button to yellow
                b.BackColor = Color.Yellow;
            }
            else
            {
                // Set colour of matched button to black
                b.BackColor = Color.Black;
            }

        }

        public bool checkStatus()
        {
            // Loop through all bools in bool array
            for (int i = 0; i < lightsMatrix.GetLength(1); i++)
            {
                for (int j = 0; j < lightsMatrix.GetLength(0); j++)
                {
                    // Check if light is on
                    if (lightsMatrix[i, j] == true)
                    {
                        // If any light is on game is still running
                        return false;
                    }
                }
            }

            // If all lights have been checked then they are all off
            return true;
        }

        public void checkEnd()
        {
            // Check if all lights are off
            if (checkStatus() == true)
            {
                // Display message to user
                MessageBox.Show("Game Completed!",
                    "Congratulations!",
                     MB_OK);
                // Close Application
                Application.Exit();
            }
        }

        private void reset_Click(object sender, EventArgs e)
        {
            GetAppSettings(URI);
        }
    }
}
