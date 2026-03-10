using UnityEditor;
using System.IO;
using System.Text.RegularExpressions;
using System.Linq;

public class EDIAutoConstansVariablesImporter : AssetPostprocessor
{
    static void OnPostprocessAllAssets(string[] imported, string[] deleted, string[] moved, string[] movedFrom)
    {
        foreach (var path in imported)
        {
            if (path.StartsWith("Assets/Scripts") && path.EndsWith(".cs"))
            {
                ProcessScript(path);
            }
        }
    }

    static void ProcessScript(string path)
    {
        string code = File.ReadAllText(path);

        // 匹配#region ConstantsUsed ... #endregion
        var constantsRegion = Regex.Match(code, @"#region ConstantsUsed([\s\S]*?)#endregion");
        var variablesRegion = Regex.Match(code, @"#region VariablesUsed([\s\S]*?)#endregion");

        if (!constantsRegion.Success && !variablesRegion.Success) return;

        // 提取变量名和类型
        var constVars = ParseRegionVars(constantsRegion.Groups[1].Value);
        var varVars = ParseRegionVars(variablesRegion.Groups[1].Value);

        // 分类
        string[] constAssignsArr = constVars.Select(v => $"{v.name} = CONS.{v.name};").ToArray();
        string[] refVarAssignsArr = varVars.Where(v => v.isListOrArray).Select(v => $"{v.name} = VARS.{v.name};").ToArray();
        string[] valueVarAssignsArr = varVars.Where(v => !v.isListOrArray).Select(v => $"{v.name} = VARS.{v.name};").ToArray();

        // Start()中#region ImportConstants
        code = ReplaceRegionAssignsInMethod(
            code, "Start", "ImportConstants", constAssignsArr);

        // Start()中#region ImportReferenceVariable
        code = ReplaceRegionAssignsInMethod(
            code, "Start", "ImportReferenceVariables", refVarAssignsArr);

        // Update()或FixedUpdate()中#region ImportValueVariables
        if (Regex.IsMatch(code, @"void\s+Update\s*\("))
        {
            code = ReplaceRegionAssignsInMethod(
                code, "Update", "ImportValueVariables", valueVarAssignsArr);
        }
        else if (Regex.IsMatch(code, @"void\s+FixedUpdate\s*\("))
        {
            code = ReplaceRegionAssignsInMethod(
                code, "FixedUpdate", "ImportValueVariables", valueVarAssignsArr);
        }

        // 保证Start/Update/FixedUpdate的大括号单独一行
        code = Regex.Replace(code, @"(void\s+Start\s*\([^\)]*\))\s*\{", "$1\n    {");
        code = Regex.Replace(code, @"(void\s+Update\s*\([^\)]*\))\s*\{", "$1\n    {");
        code = Regex.Replace(code, @"(void\s+FixedUpdate\s*\([^\)]*\))\s*\{", "$1\n    {");

        File.WriteAllText(path, code);
        AssetDatabase.Refresh();
    }

    // 替换指定方法内指定region的赋值块
    static string ReplaceRegionAssignsInMethod(
        string code, string methodName, string regionName, string[] assignsArr)
    {
        // 匹配方法体，保留大括号格式
        string pattern = $@"(void\s+{methodName}\s*\([^\)]*\)\s*\n?\{{)([\s\S]*?)(^\s*\}})";
        return Regex.Replace(code, pattern, m =>
        {
            string methodHead = m.Groups[1].Value; // 方法声明和左大括号
            string body = m.Groups[2].Value;
            string methodTail = m.Groups[3].Value; // 右大括号

            // 匹配region
            string regionPattern = $@"#region {regionName}([\s\S]*?)#endregion";
            string replacedBody = Regex.Replace(body, regionPattern, rm =>
            {
                // 去重：移除已存在的赋值
                string regionContent = RemoveAssignLines(rm.Groups[1].Value, assignsArr);
                string insert = assignsArr.Length > 0 ? "\n        " + string.Join("\n        ", assignsArr) + "\n" : "\n";
                return $"#region {regionName}{insert}        #endregion";
            });

            return $"{methodHead}{replacedBody}{methodTail}";
        }, RegexOptions.Multiline);
    }

    // 移除已存在的赋值行
    static string RemoveAssignLines(string codeBlock, string[] assignLines)
    {
        if (assignLines.Length == 0) return codeBlock;
        var lines = codeBlock.Split('\n');
        var assignSet = assignLines.Select(l => l.TrimEnd(';').Trim()).ToHashSet();
        var filtered = lines.Where(line =>
        {
            string trim = line.Trim();
            if (!trim.EndsWith(";")) return true;
            foreach (var assign in assignSet)
            {
                if (trim.StartsWith(assign) && (trim == assign + ";" || trim.StartsWith(assign + ";") || trim.StartsWith(assign + "; ")))
                    return false;
            }
            return true;
        });
        return string.Join("\n", filtered);
    }

    // 解析变量名和类型
    static (string name, string type, bool isListOrArray)[] ParseRegionVars(string region)
    {
        var lines = region.Split('\n')
            .Select(l => l.Trim())
            .Where(l => !string.IsNullOrEmpty(l) && !l.StartsWith("//"))
            .ToArray();

        var result = lines.Select(l =>
        {
            var m = Regex.Match(l, @"(\w[\w<>\[\]]*)\s+(\w+)(\s*=\s*[^;]+)?;");
            if (!m.Success) return (name: "", type: "", isListOrArray: false);
            string type = m.Groups[1].Value;
            string name = m.Groups[2].Value;
            bool isListOrArray = type.EndsWith("[]") || type.Contains("List<");
            return (name, type, isListOrArray);
        }).Where(x => !string.IsNullOrEmpty(x.name)).ToArray();

        return result;
    }
}
