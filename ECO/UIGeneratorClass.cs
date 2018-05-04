using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace ECO
{
    class UIGeneratorClass
    {
        string html;
        Dictionary<int, string> colours;
        Dictionary<int, List<double[]>> points;
        double[][] centroidsEn;
        int[] stats;
        long[] wins;
        long[] losses;
        string htmlName;
        double winLossFittness;
        double stabilityFittness;
        double weightFittness;
        double totalFittness;
        double dumbPrediction;
        public UIGeneratorClass(Dictionary<int, List<double[]>> allPoints, double[][] centroids, int[] parseStats, long[] wins, long[] losses, string htmlName, double winLossFittness, double stabilityFittness, double weightFittness, double totalFittness, double dumbPrediction)
        {
            this.wins = wins;
            this.losses = losses;


            this.stabilityFittness = stabilityFittness;
            this.weightFittness = weightFittness;
            this.totalFittness = totalFittness;
            this.winLossFittness = winLossFittness;
            this.dumbPrediction = dumbPrediction;
            this.htmlName = htmlName;
            stats = parseStats;
            centroidsEn = centroids;
            colours = new Dictionary<int, string>();
            generateColours(centroids.Length);
            points = allPoints;

            html = "<html lang=\"en\"><head><meta charset=\"utf-8\"><meta name=\"viewport\" content=\"width=device-width, initial-scale=1, shrink-to-fit=no\"><meta name=\"description\" content=\"\"><meta name=\"author\" content=\"\"><link rel=\"icon\" href=\"../../../../favicon.ico\"><title>ECO Parsing-Info</title><!-- Bootstrap core CSS --><link rel=\"stylesheet\" href=\"https://maxcdn.bootstrapcdn.com/bootstrap/4.0.0/css/bootstrap.min.css\" integrity=\"sha384-Gn5384xqQ1aoWXA+058RXPxPg6fy4IWvTNh0E263XmFcJlSAwiGgFAW/dAiS6JXm\"crossorigin=\"anonymous\"><script src=\"https://code.jquery.com/jquery-3.2.1.slim.min.js\" integrity=\"sha384-KJ3o2DKtIkvYIK3UENzmM7KCkRr/rE9/Qpg6aAZGJwFDMVNA/GpGFF93hXpG5KkN\"crossorigin=\"anonymous\"></script><script src=\"https://cdnjs.cloudflare.com/ajax/libs/popper.js/1.12.9/umd/popper.min.js\" integrity=\"sha384-ApNbgh9B+Y1QKtv3Rn7W3mgPxhU9K/ScQsAP7hUibX39j7fakFPskvXusvfa0b4Q\"crossorigin=\"anonymous\"></script><script src=\"https://maxcdn.bootstrapcdn.com/bootstrap/4.0.0/js/bootstrap.min.js\" integrity=\"sha384-JZR6Spejh4U02d8jOt6vLEHfe/JQGiRRSQQxSfFWpi1MquVdAyjUar5+76PVCmYl\"crossorigin=\"anonymous\"></script><!-- Custom styles for this template --><link href=\"album.css\" rel=\"stylesheet\"></head><body><header><div class=\"collapse bg-dark\" id=\"navbarHeader\"><div class=\"container\"><div class=\"row\"><div class=\"col-sm-8 col-md-7 py-4\"><h4 class=\"text-white\">Information overview</h4><p class=\"text-muted\">This gives some simple parsing infromation. The clusters in a 2D view, cluster statistcs and much more. For moreinformation contact the team at XXXXX Co.</p></div><div class=\"col-sm-4 offset-md-1 py-4\"><h4 class=\"text-white\">Fast Nav</h4><ul class=\"list-unstyled\"><li><a href=\"#\" class=\"text-white\">Parsing information</a></li><li><a href=\"#\" class=\"text-white\">Cluster 2D view</a></li><li><a href=\"#\" class=\"text-white\">Cluster information</a></li><li><a href=\"#\" class=\"text-white\">Conclution statistics</a></li></ul></div></div></div></div><div class=\"navbar navbar-dark bg-dark box-shadow\"><div class=\"container d-flex justify-content-between\"><a href=\"mainpage.html\" class=\"navbar-brand d-flex align-items-center\"><strong>ECO</strong></a><button class=\"navbar-toggler\" type=\"button\" data-toggle=\"collapse\" data-target=\"#navbarHeader\" aria-controls=\"navbarHeader\"aria-expanded=\"false\" aria-label=\"Toggle navigation\"><span class=\"navbar-toggler-icon\"></span></button></div></div></header><main role=\"main\"><div class=\"container\"><div class=\"jumbotron\"><h1>Parsed successfully</h1><p>Below you can see some basic informaion, mainly about the classes and parsing.</p></div><div class=\"progress\" style=\"margin-top:5%; margin-bottom:5%\"><div class=\"progress-bar bg-secondary\" role=\"progressbar\" style=\"width: 100%; height: 50%\" aria-valuenow=\"100\" aria-valuemin=\"0\"aria-valuemax=\"100\"></div></div><h6> Basic information:</h6><div class=\"row\"><div class=\"col-sm-2\"><div class=\"container mt-5\"><h4>Parsed files:</h4><h4>Errors:</h4><h4>Correlation:</h4></div></div>" + SimpleStatsGenerator() + "<div class=\"col-sm-8\"><canvas class=\"my-4 chartjs-render-monitor\" id=\"dd\" width=\"3106\" height=\"1311\" style=\"display: block; height: 874px; width: 2071px;\"></canvas></div></div><div class=\"progress\" style=\"margin-top:5%; margin-bottom:5%\"><div class=\"progress-bar bg-secondary\" role=\"progressbar\" style=\"width: 100%; height: 50%\" aria-valuenow=\"100\" aria-valuemin=\"0\"aria-valuemax=\"100\"></div></div><h6> The distance in 2D:</h6><canvas class=\"my-4 chartjs-render-monitor\" id=\"canvas\" width=\"3106\" height=\"1311\" style=\"display: block; height: 874px; width: 2071px;\"></canvas><div class=\"progress\" style=\"margin-top:5%; margin-bottom:5%\"><div class=\"progress-bar bg-secondary\" role=\"progressbar\" style=\"width: 100%; height: 50%\" aria-valuenow=\"100\" aria-valuemin=\"0\"aria-valuemax=\"100\"></div></div>" + ClusterStats() + "</div></main><footer class=\"text-muted\"><div class=\"container\"><p class=\"float-right\"><a href=\"#\">Back to top</a></p><p>This is a test overview, kind regards the coolest dude Fredrik Lindevall</p></div></footer><!-- Bootstrap core JavaScript================================================== --><!-- Placed at the end of the document so the pages load faster --><script src=\"https://code.jquery.com/jquery-3.2.1.slim.min.js\" integrity=\"sha384-KJ3o2DKtIkvYIK3UENzmM7KCkRr/rE9/Qpg6aAZGJwFDMVNA/GpGFF93hXpG5KkN\"crossorigin=\"anonymous\"></script><script src=\"https://cdnjs.cloudflare.com/ajax/libs/popper.js/1.12.9/umd/popper.min.js\" integrity=\"sha384-ApNbgh9B+Y1QKtv3Rn7W3mgPxhU9K/ScQsAP7hUibX39j7fakFPskvXusvfa0b4Q\"crossorigin=\"anonymous\"></script><script src=\"https://maxcdn.bootstrapcdn.com/bootstrap/4.0.0/js/bootstrap.min.js\" integrity=\"sha384-JZR6Spejh4U02d8jOt6vLEHfe/JQGiRRSQQxSfFWpi1MquVdAyjUar5+76PVCmYl\"crossorigin=\"anonymous\"></script><script src=\"https://cdnjs.cloudflare.com/ajax/libs/Chart.js/2.7.1/Chart.min.js\"></script><script>" + ClusterGenerator() + AmountGenerator() + ClusterStatsGenerator() + "</script><svg xmlns=\"http://www.w3.org/2000/svg\" width=\"347\" height=\"225\" viewBox=\"0 0 347 225\" preserveAspectRatio=\"none\" style=\"display: none; visibility: hidden; position: absolute; top: -100%; left: -100%;\"><defs><style type=\"text/css\"></style></defs><text x=\"0\" y=\"17\" style=\"font-weight:bold;font-size:17pt;font-family:Arial, Helvetica, Open Sans, sans-serif\">Thumbnail</text></svg></body></html>";
        }

        public void generateHTML()
        {
            System.IO.File.WriteAllText(@"..\ECO\Output\html" + htmlName + ".html", html);
        }

        private string ClusterGenerator()
        {
            string temp = "";

            temp += "var ctx = document.getElementById(\"canvas\"); var scatterChart = new Chart(ctx, { type: 'scatter', data: { datasets: [";
            int i = 1;
            string temp1;
            string temp2;
            string tempColourString;
            foreach (var key in points.Keys)
            {
                temp += "{ label: 'Cluster " + i + "', \n";

                temp += "borderColor: " + colours[key] + ", \n";



                temp += "pointBackgroundColor: " + colours[key] + ", \n";
                temp += "data: [";
                foreach (var xy in points[key])
                {
                    temp1 = Math.Round(xy[0], 5).ToString(new CultureInfo("en-US"));
                    temp2 = Math.Round(xy[1], 5).ToString(new CultureInfo("en-US"));
                    temp += "{ x: " + temp1 + ", y: " + temp2 + "}, ";
                }
                temp += "]}, \n";
                i++;

            }



            temp += "]}, options: { scales: { xAxes: [{ type: 'linear', position: 'bottom' }] } } });";


            return temp;
        }

        private string AmountGenerator()
        {
            string tempCount = "";
            string tempColours = "";
            string tempClusterName = "";
            int i = 1;
            foreach (var key in points.Keys)
            {
                tempCount += points[key].Count + ",";
                tempColours += colours[key] + ",";
                tempClusterName += "\" Cluster " + i + "\",";
                i++;
            }

            string temp = "\n \n var ctx = document.getElementById(\"dd\"); var doughnutChart = new Chart(ctx, { type: 'doughnut', data: { datasets: [{ data: [" + tempCount + " ], backgroundColor: [" + tempColours + "], label: 'Cluster Distrubution' }], labels: [" + tempClusterName + "]}, " +
                "options: { responsive: true, legend: { position: 'right', }, title: { display: false, text: 'The distrubution of classes' }, animation: { animateScale: true, animateRotate: true } } });";



            return temp;
        }

        private string ClusterStatsGenerator()
        {
            string temp = "";
            string tempValues = "";
            string tempColour = "";
            string tempLables = "";
            int i = 1;

            foreach (var ping in Enum.GetNames(typeof(PlayerData.STAT)))
            {
                tempLables += "\" CT " + ping.ToString() + "\",";
            }
            foreach (var ping in Enum.GetNames(typeof(PlayerData.STAT)))
            {
                tempLables += "\" T " + ping.ToString() + "\",";
            }

            foreach (var c in centroidsEn)
            {
                tempValues = "";
                tempColour = "";
                foreach (var value in c)
                {
                    tempValues += Math.Round(value, 5).ToString(new CultureInfo("en-US")) + ",";
                    if (value >= 0)
                        tempColour += "'rgba(30," + 255 + ",30, 1)',";
                    else
                        tempColour += "'rgba(" + 255 + ",30,30, 1)',";
                }
                temp += "\n \n var ctx = document.getElementById(\"bar" + i + "\"); var bar1Chart = new Chart(ctx, { type: 'bar', data: { datasets: [{ data: [" + tempValues + "],backgroundColor: [" + tempColour + "],}],labels: [" + tempLables + "]},options:{responsive: true, legend: false, " +
                "title: { display: false, text: 'The distrubution of classes' }, animation: { animateScale: true, animateRotate: true } } });";
                i++;
            }
            return temp;
        }


        private string SimpleStatsGenerator()
        {
            return "<div class=\"col-sm-4\"><div class=\"container mt-5\"><h4>" + stats[2] + "</h4>\n" + "<h4>" + stats[0] + "</h4>\n" + "<h4>" + stats[1] + "</h4>\n" + "<h4>" + "Statistical Prediction: " + Math.Round(dumbPrediction,5)*100 + "%</h4>\n</div></div>" + "<div class=\"col-sm-4\"><div class=\"container mt-5\"><h4>" + "Stability Fittness: " + stabilityFittness + "</h4>\n" + "<h4>" + "Win/Loss Fittness: " + winLossFittness + "</h4>\n" + "<h4>" + "Weight Fittness: " + weightFittness + "</h4>\n" +  "<h4>" + "Total Fittness: " + totalFittness + "</h4>\n</div></div>";
        }

        private string ClusterStats()
        {
            string temp = "";

            for (int x = 0; x < centroidsEn.Length; x++)
            {
                temp += "<h6> Cluster " + (x + 1) + ":</h6>" + "<canvas class=\"my-4 chartjs-render-monitor\" id=\"bar" + (x + 1) + "\" width=\"3106\" height=\"1311\" style=\"display: block; height: 874px; width: 2071px; \"></canvas>";

                long percent = (long)(((double)wins[x] / ((double)wins[x] + (double)losses[x])) * 100);
                temp += "<h6> Winrate: " + percent + "%:</h6>";
                if (wins[x] > losses[x])
                {
                    temp += "<div class=\"progress\" style=\"margin-top:2%; margin-bottom:5%\"><div class=\"progress-bar bg-success\" role=\"progressbar\" style=\"width: " + percent + "%; height: 100%\" aria-valuenow=\"" + wins[x] + "\" aria-valuemin=\"0\" aria-valuemax=\"" + (wins[x] + losses[x]) + "\"></div></div>";
                }
                else
                {
                    temp += "<div class=\"progress\" style=\"margin-top:2%; margin-bottom:5%\"><div class=\"progress-bar bg-danger\" role=\"progressbar\" style=\"width: " + percent + "%; height: 100%\" aria-valuenow=\"" + wins[x] + "\" aria-valuemin=\"0\" aria-valuemax=\"" + (wins[x] + losses[x]) + "\"></div></div>";
                }
            }
            return temp;
        }

        private void generateColours(int number)
        {
            int i = 0;
            int R = 255;
            int G = 30;
            int B = 30;
            for (int x = 0; x < number; x++)
            {
                colours.Add(x, "'rgba(" + Math.Round(R * (0.5 / (i + 1) + 0.5), 0) + "," + Math.Round(G * (0.5 / (i + 1) + 0.5), 0) + "," + Math.Round(B * (0.5 / (i + 1) + 0.5), 0) + ", 1)'");
                if ((x + 1) % 6 == 0)
                {
                    i++;
                }
                if (R == 255 && G == 30 && B == 30)
                {
                    B = 255;
                }
                else if (R == 255 && G == 30 && B == 255)
                {
                    R = 30;
                }
                else if (R == 30 && G == 30 && B == 255)
                {
                    G = 255;
                }
                else if (R == 30 && G == 255 && B == 255)
                {
                    B = 30;
                }
                else if (R == 30 && G == 255 && B == 30)
                {
                    R = 255;
                }
                else if (R == 255 && G == 255 && B == 30)
                {
                    G = 30;
                }
            }
        }

    }
}

