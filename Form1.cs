using static System.Net.Mime.MediaTypeNames;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Globalization;

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
            if (i_primer_length.Text.Length == 0 || i_min_temp.Text.Length == 0 || i_max_temp.Text.Length == 0 || i_DNA.Text.Length == 0) {
                MessageBox.Show("All input fields are mandatory!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            string DNA = i_DNA.Text;

            //parsing ints
            if (!int.TryParse(i_primer_length.Text, out int primer_length) || !double.TryParse(i_min_temp.Text, NumberStyles.Any, CultureInfo.InvariantCulture, out double min_temp) || !double.TryParse(i_max_temp.Text, NumberStyles.Any, CultureInfo.InvariantCulture, out double max_temp))
            {
                MessageBox.Show("Please enter valid numbers.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            
            List<string> valid_strings = this.GetValidSubstrings(DNA, primer_length);
            List<Primer> primer_list = new List<Primer>();

            foreach (var valid_sequence in  valid_strings)
            {
                float melting = this.CalculateMeltingTemp(valid_sequence);
                if (melting < min_temp || melting > max_temp)
                    continue;

                float delta_g = this.CalculateDeltaG(valid_sequence);
                Primer primer = new Primer(valid_sequence, melting, delta_g);
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

                //gc percentage >50%
                float gc_count = 0.0f;
                for (int j = 0; j < substring.Length; j++)
                {
                    if (substring[j] == 'G' || substring[j] == 'C')
                        gc_count++;
                }
                if ((gc_count / length) < 0.5)
                    continue;

                substrings.Add(substring);
            }

            return substrings;
        }

        private float CalculateMeltingTemp(string sequence)
        {
            //Placeholder calculation for melting temperature
            return 60.0f;
        }

        private float CalculateDeltaG(string sequence)
        {
            //Placeholder calculation for delta G
            return -5.0f;
        }

        private void SavePrimersToCsv(string csv_path, List<Primer> primers)
        {
            

            using (StreamWriter writer = new StreamWriter(csv_path))
            {
                //write CSV header
                writer.WriteLine("melting_temp,delta_g,sequence");

                //write primer details
                foreach (var primer in primers)
                {
                    writer.WriteLine($"{primer.melting_temp},{primer.delta_g},{primer.sequence}");
                }
            }
        }
    }
}
