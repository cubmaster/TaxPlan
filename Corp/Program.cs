using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;
using System.Threading.Tasks.Dataflow;
using Jace;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RulesEngine.Models;
using static RulesEngine.Extensions.ListofRuleResultTreeExtension;
namespace Corp
{
    class Program
    {

        private static JObject o2;
        static void Main(string[] args)
        {
           // SeperateFiles();
           RecusiveCalc();

        }

        static void RecusiveCalc()
        {

            var workflowFiles = Directory.GetFiles($"{Directory.GetCurrentDirectory()}/Scenarios", "custom.json",
                SearchOption.AllDirectories);

            using (StreamReader file = File.OpenText($"{Directory.GetCurrentDirectory()}/Scenarios/custom.json"))
            using (JsonTextReader reader = new JsonTextReader(file))
                { 
                    o2 = (JObject)JToken.ReadFrom(reader);
                    var calc =  (string)o2.SelectToken("Start");
                    var result =ParseCalc(calc);
                    Console.WriteLine(result);
                }

            Console.Read();
        }

        static string ParseCalc(string calc)
        {
            var subjectCalc  = calc;
            var terms = calc.Split(new char[] { '+', '-', '/', '*' });
            for (var i = 0; i < terms.Length; i++)
            {
                
                var node = o2.SelectToken(terms[i]);
                if (node != null)
                {
                    var result = ParseCalc(node.Value<string>());
                    subjectCalc=subjectCalc.Replace(terms[i], result);
                }
                else
                {
                    return subjectCalc;
                }
            
            }
            CalculationEngine engine = new CalculationEngine();
            try
            {
                
                double calcResult = engine.Calculate(subjectCalc);
                var stringResult =calcResult.ToString("#");
                Console.WriteLine($"{calc} -> {subjectCalc} = {stringResult}");
                return stringResult;
            }
            catch (Exception e)
            {
                throw new Exception($"{calc} returned {subjectCalc} and could not be computed");
            }
       
      
        }
        
    }
}