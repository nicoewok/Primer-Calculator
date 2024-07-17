using static System.Net.Mime.MediaTypeNames;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Globalization;
using System.Data.SqlTypes;

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

                float melting = this.CalculateMeltingTemp(valid_sequence.Length, gc_count);
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
                } else {
                    last_letter = text[i];
                    letters_in_row = 0;
                }

                if (letters_in_row == 2)
                    return false;
            }
           
            return true;
        }

        private float CalculateMeltingTemp(int length, float gc_count)
        {
            //Placeholder calculation for melting temperature
            return 81.5f + (0.41f * gc_count) - (675.0f / (float)length);
            //return 60f;
        }

        private float CalculateDeltaG(string sequence)
        {
            //Placeholder calculation for delta G
            return -5.0f;
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

        private void button1_Click(object sender, EventArgs e)
        {
            //test
            string test1 = "CGCCC";
            string test2 = "GCGC";
            string test3 = "TAAAA";
            string test4 = "TATAAT";

            Console.WriteLine("Should be:\nno\nyes\nno\nyes\nBut is:\n\n");

            string bools = $"{this.CheckMultipleLetter(test1)}\n{this.CheckMultipleLetter(test2)}\n{this.CheckMultipleLetter(test3)}\n{this.CheckMultipleLetter(test4)}";
            MessageBox.Show($"Should be:\nno\nyes\nno\nyes\nBut is:\n\n{bools}", "Success!", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
