using BatchToDoCLI.Models;
using System.Text.RegularExpressions;

namespace BatchToDoCLI.Definitions
{
    public class Transformer
    {
        private Regex VariableMatcher;

        public Transformer()
        {
            VariableMatcher = new Regex("{{\\w+}}");
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
            newTask.DueDate = FillDateExpression(original.DueDate, vars);

            return newTask;
        }

        private string FillTextVariables(string original, Variables vars)
        {
            var matches = VariableMatcher.Matches(original);

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
            // todo: implement

            return original;
        }
    }
}
