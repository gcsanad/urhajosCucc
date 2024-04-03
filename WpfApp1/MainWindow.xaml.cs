using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using System.Windows.Shapes;
using HelixToolkit;
using HelixToolkit.Wpf;
using Microsoft.Win32;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<Bolygo> bolygok = new();
        
        int tavolsag = 5;
        Random rnd = new Random();
        int index = 1;
        Point3D elso;
        EllipsoidVisual3D kezdoBolygo;
        EllipsoidVisual3D vegBolygo;
        public MainWindow()
        {
            InitializeComponent();
            ter.IsHeadLightEnabled = true;

        }
        
        private void btnBetolt_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();

            if (ofd.ShowDialog() == true)
            {
                var ut = System.IO.Path.GetExtension(ofd.FileName);
                bolygok = File.ReadAllLines(ofd.FileName.ToString()).Skip(1).Select(x => new Bolygo(x)).ToList();

            }
            ter.Children.Clear();
            TerBetoltese();
            
        }

        private void TerBetoltese()
        {
            
            foreach (var bolygo in bolygok)
            {
                
                EllipsoidVisual3D bolygo3D = new EllipsoidVisual3D();
                bolygo3D.RadiusX = bolygo.Ertek;
                bolygo3D.RadiusY = bolygo.Ertek;
                bolygo3D.RadiusZ = bolygo.Ertek;
                bolygo3D.Center = new Point3D(bolygo.X * tavolsag, bolygo.Y * tavolsag, bolygo.Z * tavolsag);
                /*Color.FromRgb((byte)rnd.Next(0,255), (byte)rnd.Next(0, 255), (byte)rnd.Next(0, 255))*/
                bolygo3D.Fill = new SolidColorBrush(Colors.Red);
                ter.Children.Add(bolygo3D);

            }
        }

        private void ter_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Point mousePosition = e.GetPosition(ter);

            HitTestResult hitTestResult = VisualTreeHelper.HitTest(ter, mousePosition);
            if (hitTestResult != null && hitTestResult.VisualHit is EllipsoidVisual3D)
            {
                EllipsoidVisual3D clickedBolygo = (EllipsoidVisual3D)hitTestResult.VisualHit;

                if (index == 1)
                {
                    elso = clickedBolygo.Center;
                    kezdoBolygo = clickedBolygo;
                    index++;
                }
                else if (index == 2)
                {
                    var line = new LinesVisual3D
                    {
                        Points = new Point3DCollection { clickedBolygo.Center, elso },
                        Thickness = 2,
                        Color = Colors.Green,

                    };
                    vegBolygo = clickedBolygo;
                    ter.Children.Add(line);
                    elso = clickedBolygo.Center;
                    index = 0;
                    
                }
            }
            
            
        }
         double Tavolsag(IBolygo egyik, IBolygo masik)
        {
            return Math.Sqrt(Math.Pow(egyik.X - masik.X, 2) + Math.Pow(egyik.Y - masik.Y, 2) + Math.Pow(egyik.Z - masik.Z, 2));

        }
        Bolygo Cel;
        const double FOGYASZTAS = 2;
        const int AKKUMULATOR = 120;
            
        double hatotav = AKKUMULATOR / FOGYASZTAS;

        List<Bolygo> utvonal = new();
        List<Bolygo> rosszak = new();
        private void brutal() 
        {
            Cel = bolygok.Last();
            var eredmeny = Tovabblep(new List<Bolygo>(), bolygok.First());
            if (CelbaErt(eredmeny.Last(),Cel))
            {
                MessageBox.Show($"Sikeresen célba ért! Útvonal hossza: {eredmeny.Zip(eredmeny.Skip(1), ())=> Tavolsag().Sum()}");
            }
        }

        private bool CelbaErt(Bolygo honnan, Bolygo hova) => honnan.X == hova.X && honnan.Y == hova.Y && honnan.Z == hova.Z;

        private List<Bolygo> Tovabblep(List<Bolygo> utvonal, Bolygo ujHely)
        {
            
            utvonal.Add(ujHely);
            if (CelbaErt(ujHely, Cel))
            {
                return utvonal;
            }

            IEnumerable<Bolygo> elerhetok = bolygok.Where(x => Tavolsag(utvonal.Last(), x) < hatotav);

            List<Bolygo> valaszthatok = elerhetok.Except(utvonal).Except(rosszak).ToList();

            if (valaszthatok.Count() == 0)
            {
                return utvonal;
            }

            double EddigiMiniUthossz = double.MaxValue;
            List<Bolygo> talaltLegrovidebbUt = new();
            foreach (var merre in valaszthatok)
            {
                List<Bolygo> eredmeny = Tovabblep(utvonal, merre);
                double utHossza = eredmeny.Zip(eredmeny.Skip(1), eredmeny.Zip(eredmeny.Skip(1), ())=> Tavolsag().Sum());
                if (utHossza <EddigiMiniUthossz&&CelbaErt(eredmeny.Last(),Cel))
                {
                    EddigiMiniUthossz = utHossza;
                    talaltLegrovidebbUt = eredmeny.ToList();
                }
                else
                {
                    rosszak.AddRange(eredmeny);
                }
            }


            return talaltLegrovidebbUt;
        }
    }
}