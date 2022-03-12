using BatchToDoCLI.Exceptions;
using BatchToDoCLI.Models;
using System.Text.RegularExpressions;

namespace BatchToDoCLI.Definitions
{
    public class Transformer
    {
        private Regex StrVariableMatcher;
        private Regex DateVariableMatcher;

        public Transformer()
        {
            StrVariableMatcher = new Regex("{{\\w+}}");
            DateVariableMatcher = new Regex("{{\\w+[+|-]\\d+[d|w|m|y]}}");
        }

        public TaskBatch TransformFromVariables(TaskBatch template, Variables userVars)
        {
            var transformed = new TaskBatch();

            transformed.BatchName = FillTextVariables(template.BatchName, userVars);
            transformed.Tasks = new List<TaskItem>();

            foreach (var item in template.Tasks)
            {
                transformed.Tasks.Add(FillTaskItemVariables(item, userVars));
            }

            return transformed;
        }

        private TaskItem FillTaskItemVariables(TaskItem original, Variables vars)
        {
            var newTask = new TaskItem();
            
            newTask.Name = FillTextVariables(original.Name, vars);
            newTask.Description = FillTextVariables(original.Description, vars);
            newTask.DueDate = original.DueDate;
            newTask.Evaluated = FillDateExpression(original.DueDate, vars);

            return newTask;
        }

        private string FillTextVariables(string original, Variables vars)
        {
            var matches = StrVariableMatcher.Matches(original);

            if (matches.Count == 0)
            {
                return original;
            }

            var filledString = original;

            foreach (Match match in matches)
            {
                string varKey = match.Value.Substring(2, match.Value.Length - 4);

                if (vars.ContainsKey(varKey))
                {
                    filledString = filledString.Replace(match.Value, vars[varKey]);
                }
            }

            return filledString;
        }

        private string FillDateExpression(string original, Variables vars)
        {
            var matches = DateVariableMatcher.Matches(original);
            if (matches.Count == 0)
            {
                return original;
            }

            var filledString = original;

            foreach (Match match in matches)
            {
                string varKey = match.Value.Substring(2, match.Value.Length - 4);
                var varAndExp = varKey.Split('-', '+');
                char splitChar = match.Value.Substring(varAndExp[0].Length + 2, 1).FirstOrDefault();

                if (vars.ContainsKey(varAndExp[0]))
                {
                    try
                    {
                        filledString = EvaluateDateExpression(vars[varAndExp[0]], varAndExp[1], splitChar);
                    }
                    catch (Exception)
                    {
                        throw new InvalidDateExpressionException($"Couldn't parse the date expression: {match.Value}");
                    }
                }
            }

            return filledString;
        }

        private string EvaluateDateExpression(string date, string expression, char symbol)
        {
            DateTime parsedDate = DateTime.Parse(date);

            var modifierType = expression.Last();
            var modifierValue = expression.Take(expression.Length - 1).ToString();
            var modifierAsInt = int.Parse(modifierValue);

            switch (modifierType)
            {
                case 'd':
                    {
                        break;
                    }
                case 'w':
                    {
                        break;
                    }
                case 'm':
                    {
                        break;
                    }
                case 'y':
                    {
                        break;
                    }
                default:
                    {
                        throw new NotImplementedException($"Unexpected modifier type: {modifierType}");
                    }
            }

            return "";
        }
    }
}
