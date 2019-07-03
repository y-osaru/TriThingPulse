using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.Diagnostics;

namespace TriThingPulse
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //3つのWi-Fiのアクセスポイント(AP)からの距離を三点測位の計算式より算出
            //各APの座標を与える事で、端末の位置を直交座標系で算出する。
            //3つの波という事でTriThingPulse。他意はない。

            //まぁ長く使うもんでもないし……動けばよかろ。

            //AとBとRSSI(r)からAPからの距離を計算する
            double distApA = 0.0;
            double distApB = 0.0;
            double distApC = 0.0;
            try {
                distApA = Math.Pow(10.0, (double.Parse(AApA.Text) - double.Parse(RrApA.Text)) / (10.0 * double.Parse(B.Text)));
                distApB = Math.Pow(10.0, (double.Parse(AApB.Text) - double.Parse(RrApB.Text)) / (10.0 * double.Parse(B.Text)));
                distApC = Math.Pow(10.0, (double.Parse(AApC.Text) - double.Parse(RrApC.Text)) / (10.0 * double.Parse(B.Text)));
            }
            catch (Exception ex) {
                resultBox.Text = ex.Message + "\r\n";
            }

            //デバック時(1.0, 0.0)が交点になる
            #if DEBUG
                XApA.Text = Convert.ToString(0);
                YApA.Text = Convert.ToString(0);
                XApB.Text = Convert.ToString(-3);
                YApB.Text = Convert.ToString(0);
                XApC.Text = Convert.ToString(2);
                YApC.Text = Convert.ToString(0);
                distApA = 1.0;
                distApB = 2.0;
                distApC = 3.0;
            #endif

            //結果のboxに表示
            resultBox.Text = "各APからの距離は\r\n";
            resultBox.Text += "AP-A:" + distApA + "\r\n";
            resultBox.Text += "AP-B:" + distApB + "\r\n";
            resultBox.Text += "AP-C:" + distApC + "\r\n";


            //フォームの入力内容と共に、pythonのスクリプトを実行。
            //引数はそれぞれのAPのx,y,それぞれの距離(xA yA rA xB yB rB xC yC rC)
            Process p = new Process();
            p.StartInfo.FileName = Environment.GetEnvironmentVariable("ComSpec");
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.RedirectStandardInput = false;
            p.StartInfo.CreateNoWindow = true;
            p.StartInfo.Arguments = @"/c python script/CalcGridPosition.py " + string.Join(" ", XApA.Text, YApA.Text, distApA, XApB.Text, YApB.Text, distApB, XApC.Text, YApC.Text, distApC);

            resultBox.Text += "execute cmd: " + p.StartInfo.Arguments.ToString() + "\r\n";

            //起動
            p.Start();

            //出力を読み取る
            string results = p.StandardOutput.ReadToEnd();
            p.WaitForExit();
            p.Close();

            //結果を取得
            resultBox.Text += "result: " + results;
        }
    }
}
