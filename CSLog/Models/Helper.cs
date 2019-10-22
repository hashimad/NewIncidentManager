using DotNet.Highcharts;
using DotNet.Highcharts.Enums;
using DotNet.Highcharts.Helpers;
using DotNet.Highcharts.Options;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace CSLog.Models
{
    public class Helper
    {
        public string getMsg(string alertType, string Msg)
        {
            return $"<div class=\"alert alert-{alertType}\"> <strong>{alertType}!</strong> <span data-dismiss=\"alert\" class=\"close\">&times;</span> { Msg}</div>";
        }
        public bool IsValidEmail(string email)
        {
            return Regex.IsMatch(email.Trim(), "^\\w+([-+.']\\w+)*@\\w+([-.]\\w+)*\\.\\w+([-.]\\w+)*$");
        }

        public bool IsValidPhone(string data)
        {
            return Regex.IsMatch(data.Trim(), "^[0]\\d{10}$");
        }

        public Highcharts GetChart(ChartTypes chartType, string title, string[] categories, Series[] series,
            Color bgColor, Color borderColor, string subTitle = "", string xAxistTitle = "", string yAxistTitle = "")
        {
            Highcharts columnChart = new Highcharts("columnchart");
            columnChart.InitChart(new Chart()
            {
                Type = chartType,
                BackgroundColor = new BackColorOrGradient(bgColor),
                Style = "fontWeight: 'bold', fontSize: '12px'",
                BorderColor = borderColor,
                BorderRadius = 0,
                BorderWidth = 1
                
            });


            columnChart.SetTitle(new Title()
            {
                Text = title
            });

            columnChart.SetSubtitle(new Subtitle()
            {
                Text = subTitle
            });

            columnChart.SetXAxis(new XAxis()
            {
                Type = AxisTypes.Category,
                Title = new XAxisTitle() { Text = xAxistTitle, Style = "fontWeight: 'bold', fontSize: '12px'" },
                Categories = categories //new[] { "2004", "2005", "2006", "2007", "2008", "2009", "2010", "2011", "2012" }
            });

            columnChart.SetYAxis(new YAxis()
            {
                Title = new YAxisTitle()
                {
                    Text = yAxistTitle,
                    Style = "fontWeight: 'bold', fontSize: '12px'"
                },
                ShowFirstLabel = true,
                ShowLastLabel = true,
                Min = 0,
                AllowDecimals = false
            });

            columnChart.SetLegend(new Legend
            {
                Enabled = true,
                BorderColor = Color.CornflowerBlue,
                BorderRadius = 6,
                BackgroundColor = new BackColorOrGradient(Color.Transparent)//ColorTranslator.FromHtml("#FFADD8E6"))
            });

            columnChart.SetSeries(series);
            return columnChart;
        }
    }
}
public enum AlertType
{
    success,
    danger,
    warning
}
