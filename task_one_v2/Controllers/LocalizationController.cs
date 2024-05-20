using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using task_one_v2.Models;

namespace task_one_v2.Controllers
{
    public class LocalizationController : Controller
    {
        private readonly string arabicFilePath = "Resources/ar-JO.json";
        private readonly string englishFilePath = "Resources/en-US.json";

        public IActionResult LangIndex()
        {
            var arabicData = ReadJsonFile(arabicFilePath);
            var englishData = ReadJsonFile(englishFilePath);
            var entries = arabicData.Select(kv => new LocalizationEntry
            {
                Key = kv.Key,
                ArabicValue = kv.Value,
                EnglishValue = englishData[kv.Key]
            }).ToList();

            return View(entries);
        }

        







        [HttpPost]
        public IActionResult Update(List<LocalizationEntry> entries)
        {
            if (ModelState.IsValid)
            {
                foreach (var entry in entries)
                {
                    UpdateLocalizationEntry(entry.Key, entry.ArabicValue, entry.EnglishValue);
                }
            }

            return RedirectToAction("LangIndex");
        }



        private void UpdateLocalizationEntry(string key, string arabicValue, string englishValue)
        {
            var arabicData = ReadJsonFile(arabicFilePath);
            var englishData = ReadJsonFile(englishFilePath);

            arabicData[key] = arabicValue;
            englishData[key] = englishValue;

            WriteJsonFile(arabicFilePath, arabicData);
            WriteJsonFile(englishFilePath, englishData);
        }


        private Dictionary<string, string> ReadJsonFile(string filePath)
        {
            var jsonString = System.IO.File.ReadAllText(filePath);
            return JsonConvert.DeserializeObject<Dictionary<string, string>>(jsonString);
        }
        private void WriteJsonFile(string filePath, Dictionary<string, string> data)
        {
            var jsonString = JsonConvert.SerializeObject(data, Formatting.Indented);
            System.IO.File.WriteAllText(filePath, jsonString);
        }








        //
        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Add(LocalizationEntry entry)
        {
            if (ModelState.IsValid)
            {
                AddLocalizationEntry(entry.Key, entry.ArabicValue, entry.EnglishValue);
                return RedirectToAction("LangIndex");
            }
            return View(entry);
        }

        private void AddLocalizationEntry(string key, string arabicValue, string englishValue)
        {
            var arabicData = ReadJsonFile(arabicFilePath);
            var englishData = ReadJsonFile(englishFilePath);

            arabicData[key] = arabicValue;
            englishData[key] = englishValue;

            WriteJsonFile(arabicFilePath, arabicData);
            WriteJsonFile(englishFilePath, englishData);
        }



        //delete
        [HttpPost]
        public IActionResult Delete(string key)
        {
            if (ModelState.IsValid)
            {
                DeleteLocalizationEntry(key);
            }
            return RedirectToAction("LangIndex");
        }

        private void DeleteLocalizationEntry(string key)
        {
            var arabicData = ReadJsonFile(arabicFilePath);
            var englishData = ReadJsonFile(englishFilePath);

            if (arabicData.ContainsKey(key))
            {
                arabicData.Remove(key);
            }

            if (englishData.ContainsKey(key))
            {
                englishData.Remove(key);
            }

            WriteJsonFile(arabicFilePath, arabicData);
            WriteJsonFile(englishFilePath, englishData);
        }

    }
}
