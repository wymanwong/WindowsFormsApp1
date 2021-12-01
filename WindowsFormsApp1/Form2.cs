using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.ML;
using Microsoft.ML.Data;
using Microsoft.ML.Runtime.Api;
using Microsoft.ML.Trainers;
using Microsoft.ML.Transforms;

namespace WindowsFormsApp1
{
    public partial class Form2 : Form
    {

        private int _ticks;
        private int deleteValue;
        private int press2key;
        private int keyValue;

        private List<int> termsList = new List<int>();
        public Form2()
        {
            InitializeComponent();
            timer1.Start();

        }

        public void Predictionfunction(int HotKey,int Backspace,int Press2Keys,int KeyInTime)
        {
            textBox1.Text = "";
            var pipeline = new LearningPipeline();

          //  MessageBox.Show(HotKey.ToString()+"/" + Backspace.ToString() + "/" + Press2Keys.ToString() + "/" + KeyInTime.ToString());


            string dataPath = "test.txt";
            pipeline.Add(new TextLoader(dataPath).CreateFrom<IrisData>(separator: ','));


            //transform data
            pipeline.Add(new Dictionarizer("Label"));
            pipeline.Add(new ColumnConcatenator("Features", "HotKey", "Backspace", "Press2Keys", "KeyInTime"));


            //add a learning algorithm
            pipeline.Add(new StochasticDualCoordinateAscentClassifier());
            pipeline.Add(new PredictedLabelColumnOriginalValueConverter() { PredictedLabelColumn = "PredictedLabel" });


            //train the model
            var model = pipeline.Train<IrisData, IrisPrediction>();

            //use your model to make prediction


            var prediction = model.Predict(new IrisData()
            {
                HotKey = HotKey,
                Backspace = Backspace,
                Press2Keys = Press2Keys,
                KeyInTime = KeyInTime


            });

            textBox1.Text = "Pridicted login class is :" + prediction.PredictedLabels;
           
        }
        private void textBox3_KeyDown(object sender, KeyEventArgs e)
        {
   

            if (e.Alt == true || e.Control == true || e.KeyCode == Keys.Back || e.KeyValue == 37 || e.KeyValue == 38 || e.KeyValue == 39 || e.KeyValue == 40)
            {
                keyValue = e.KeyValue;
                termsList.Add(keyValue);
         
                
            }
            else if (e.Shift == true && e.KeyValue != 0)
                press2key = 2;
           

            else
                keyValue = 0;

        }

        private void button1_Click(object sender, EventArgs e)
        {
            int[] terms = termsList.ToArray();
            for (int run = 0; run < terms.Length; run++)
            {
                if (terms[run] > 0 && terms[run] != 8)
                  keyValue = terms[run];

                else
                    deleteValue = terms[run];
             
            }

            //textBox3.Text = "human"; //login time > 8 seconds, keyin using Backspace, Alt,Option and press 2 key at same time
         //   if (_ticks < 8 && keyValue == 0 && deleteValue == 0 &&press2key==2)
          //   MessageBox.Show("Submit "+keyValue.ToString());
         
            Predictionfunction(keyValue, deleteValue, press2key,_ticks);

            termsList.Clear();
            press2key = 0;
            keyValue = 0;
            deleteValue = 0;
            timer1.Stop();
            timer1.Start();
            _ticks = 0;

            MessageBox.Show(" Show the predicted results ");
            textBox3.Text = "";

        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            _ticks++;
            this.Text = _ticks.ToString();

        }

    }

       


        public class IrisData
        {
            [Column("0")]
            public string Label;

            [Column("1")] 
            public float HotKey;

            [Column("2")] 
            public float Backspace;

            [Column("3")]
            public float Press2Keys;

            [Column("4")] 
            public float KeyInTime;

        }

      
        public class IrisPrediction
        {
            [ColumnName("PredictedLabel")]
            public string PredictedLabels;
        }

     


    }

