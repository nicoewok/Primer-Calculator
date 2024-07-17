using static System.Net.Mime.MediaTypeNames;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Globalization;
using System.Data.SqlTypes;
using static System.Reflection.Metadata.BlobBuilder;

namespace PrimerCalculator
{
    public partial class Form1 : Form
    {

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void btn_calculate_Click(object sender, EventArgs e)
        {
            //getting the parameters
            if (i_primer_length.Text.Length == 0 || i_min_temp.Text.Length == 0 || i_max_temp.Text.Length == 0 || i_DNA.Text.Length == 0)
            {
                MessageBox.Show("All input fields are mandatory!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            string DNA = i_DNA.Text.ToUpper();

            //parsing ints
            if (!int.TryParse(i_primer_length.Text, out int primer_length) || !double.TryParse(i_min_temp.Text, NumberStyles.Any, CultureInfo.InvariantCulture, out double min_temp) || !double.TryParse(i_max_temp.Text, NumberStyles.Any, CultureInfo.InvariantCulture, out double max_temp))
            {
                MessageBox.Show("Please enter valid numbers.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            List<string> valid_strings = this.GetValidSubstrings(DNA, primer_length);
            List<Primer> primer_list = new List<Primer>();

            foreach (var valid_sequence in valid_strings)
            {
                //gc percentage >50%
                float gc_count = this.CalculateGCCount(valid_sequence);
                if (gc_count < 0.5f)
                    continue;

                float melting = this.CalculateMeltingTemp(valid_sequence);
                //if (melting < min_temp || melting > max_temp)
                //    continue;




                float delta_g = this.CalculateDeltaG(valid_sequence);
                Primer primer = new Primer(valid_sequence, melting, delta_g, gc_count);
                primer_list.Add(primer);
            }

            //sort list and cut to top 10
            List<Primer> top_primers = primer_list.OrderBy(p => p.delta_g).Take(10).ToList();

            //save to csv
            string exe_path = AppDomain.CurrentDomain.BaseDirectory;
            string csv_path = Path.Combine(exe_path, "primers.csv");
            this.SavePrimersToCsv(csv_path, top_primers);


            MessageBox.Show($"We found a primer!\nThe top 10 have been saved to:\n{csv_path}", "Success!", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }


        private List<string> GetValidSubstrings(string text, int length)
        {
            List<string> substrings = new List<string>();

            for (int i = 0; i <= text.Length - length; i++)
            {
                string substring = text.Substring(i, length);

                //start/end with G/C
                if ((!substring.StartsWith("C") && !substring.StartsWith("G")) || !(substring.EndsWith("C") && !substring.EndsWith("G")))
                    continue;

                //check if letter more than 2 times in a row
                if (!this.CheckMultipleLetter(substring))
                    continue;

                substrings.Add(substring);
            }

            return substrings;
        }

        //checks if more than 2 times
        //return true if it is ok, false if more than 2 in a row
        private bool CheckMultipleLetter(string text)
        {
            char last_letter = '0';
            int letters_in_row = 0;
            for (int i = 0; i < text.Length; i++)
            {
                if (text[i] == last_letter)
                {
                    letters_in_row++;
                }
                else
                {
                    last_letter = text[i];
                    letters_in_row = 0;
                }

                if (letters_in_row == 2)
                    return false;
            }

            return true;
        }

        private float CalculateMeltingTemp(string sequence)
        {
            //Placeholder calculation for melting temperature
            return this.CalculateDeltaH(sequence)/this.CalculateDeltaS(sequence) - 273.15f;
            //return 60f;
        }

        private float CalculateDeltaG(string sequence)
        {
            //Placeholder calculation for delta G
            float delta_g = this.CalculateDeltaH(sequence) - 298.15f * this.CalculateDeltaS(sequence); //it's in cal/mol
            return delta_g/1000;
        }


        private float CalculateGCCount(string sequence)
        {
            //gc percentage >50%
            float gc_count = 0.0f;
            for (int j = 0; j < sequence.Length; j++)
            {
                if (sequence[j] == 'G' || sequence[j] == 'C')
                    gc_count++;
            }
            return gc_count / sequence.Length;
        }


        private void SavePrimersToCsv(string csv_path, List<Primer> primers)
        {


            using (StreamWriter writer = new StreamWriter(csv_path))
            {
                //write CSV header
                writer.WriteLine("melting_temp,delta_g,gc_count,sequence");

                //write primer details
                foreach (var primer in primers)
                {
                    string melting = primer.melting_temp.ToString("0.0", CultureInfo.InvariantCulture);
                    string delta = primer.delta_g.ToString("0.0", CultureInfo.InvariantCulture);
                    string gc = primer.gc_count.ToString("0.0", CultureInfo.InvariantCulture);
                    writer.WriteLine($"{melting},{delta},{gc},{primer.sequence}");
                }
            }
        }


        private int GetIndexByBase(char b)
        {
            switch (b)
            {
                case 'A':
                    return 0;
                case 'C':
                    return 1;
                case 'G':
                    return 2;
                case 'T':
                    return 3;
                default:
                    return 4;
            }
        }

        private float GetEnthalpy(string neighbor)
        {
            float[,] enthalphies = { { 9100, 6500, 7800, 8600 }, { 5800, 11000, 11900, 7800 }, { 5600, 11100, 11000, 6500 }, { 6000, 5600, 5800, 9100 } };
            int i = this.GetIndexByBase(neighbor[0]);
            int j = this.GetIndexByBase(neighbor[1]);

            return enthalphies[i, j];
        }

        private float CalculateDeltaH(string sequence)
        {
            float delta_h = 0.0f;
            for (int i = 0; i <= sequence.Length - 2; i++)
            {
                string neighbor = sequence.Substring(i, 2);

                delta_h += this.GetEnthalpy(neighbor);
            }

            return delta_h;
        }

        private float GetEntropy(string neighbor)
        {
            float[,] entropies = { { 24.0f, 17.3f, 20.8f, 23.9f }, { 12.9f, 26.6f, 27.8f, 20.8f }, { 13.5f, 26.7f, 26.6f, 17.3f }, { 16.9f, 13.5f, 12.9f, 24.0f} };
            int i = this.GetIndexByBase(neighbor[0]);
            int j = this.GetIndexByBase(neighbor[1]);

            return entropies[i, j];
        }

        private float CalculateDeltaS(string sequence)
        {
            float delta_s = 15.1f;
            for (int i = 0; i <= sequence.Length - 2; i++)
            {
                string neighbor = sequence.Substring(i, 2);

                delta_s += this.GetEntropy(neighbor);
            }
            
            return delta_s;
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            MessageBox.Show($"For CG it's {this.GetEnthalpy("CG")}", "Success!", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
