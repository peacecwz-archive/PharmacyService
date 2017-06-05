using HtmlAgilityPack;
using PharmacyService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;

namespace PharmacyService.Services
{
    public class PharmacyService
    {
        #region Singleton

        private static PharmacyService _instance;
        public static PharmacyService Instance
        {
            get { return (_instance == null) ? _instance = new PharmacyService() : _instance; }
        }
        private PharmacyService() { }
        #endregion

        public async Task<List<Pharmacy>> GetList(string city)
        {
            string url = $"http://www.hastanebul.com.tr/{city}-nobetci-eczaneler";
            using (HttpClient client = new HttpClient())
            {
                var resposne = await client.GetAsync(url);
                if (resposne != null && resposne.IsSuccessStatusCode)
                {
                    string html = await resposne.Content?.ReadAsStringAsync();
                    return ParseList(html);
                }
            }
            return null;
        }

        private List<Pharmacy> ParseList(string html)
        {
            List<Pharmacy> pharmacies = new List<Pharmacy>();

            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(html);

            var divs = doc.DocumentNode.SelectNodes("//div[@class='panel panel-default']");
            if (divs.Count == 0) return pharmacies;
            foreach (var div in divs)
            {
                Pharmacy pharmacy = new Pharmacy();
                pharmacy.Name = div.SelectSingleNode(".//div[@class='panel-heading']").InnerText.Trim().SoftClearText();
                if (string.IsNullOrEmpty(pharmacy.Name)) continue;
                string body = div.SelectSingleNode(".//div[@class='panel-body pharmacyonduty']").InnerHtml;
                body = body.Substring(body.IndexOf("</b>")+4);
                pharmacy.Address = body.Substring(body.IndexOf("<br>")).Replace("<br>", "");
                pharmacy.Address = pharmacy.Address.Substring(0, pharmacy.Address.IndexOf("<b>")).Replace("<b>", "");
                pharmacy.Phone = body.Substring(body.IndexOf("</b>")).Replace("</b>", "");
                pharmacy.Phone = pharmacy.Phone.Replace(":", "").Replace("<br>","").Trim();
                pharmacies.Add(pharmacy);
            }

            return pharmacies;
        }
    }
}