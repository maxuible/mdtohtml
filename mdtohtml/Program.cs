
using System;
using System.Diagnostics;

const string dir = "C:\\temp\\";

const string header = """
<!DOCTYPE html>
<html>
<head>
    <title></title>
</head><!DOCTYPE html>
<html>
<head>
    <title></title>
</head>
<body>
""";

const string foot = """
</body>
</html>
""";

const string md_h1 = "# ";
const string md_h2 = "## ";
const string md_h3 = "### ";
const string md_h4 = "#### ";

string filePath = dir + "test.md";
string[] lines = File.ReadAllLines(filePath);


bool inList = false;



using (StreamWriter writer = new StreamWriter(dir + "test.html"))
{
    writer.WriteLine(header);
    for (int i = 0; i < lines.Length; i++) 
    {
        string line = lines[i];
        if (line.StartsWith(md_h1))
        {
            writer.WriteLine("<h1>");
            int index = line.IndexOf(md_h1);
            writer.WriteLine(line.Substring(index + md_h1.Length, line.Length - md_h1.Length));
            writer.WriteLine("</h1>");
        } else if (line.StartsWith(md_h2))
        {
            writer.WriteLine("<h2>");
            int index = line.IndexOf(md_h2);
            writer.WriteLine(line.Substring(index + md_h2.Length, line.Length - md_h2.Length));
            writer.WriteLine("</h2>");
        } else if (line.StartsWith(md_h3))
        {
            writer.WriteLine("<h3>");
            int index = line.IndexOf(md_h3);
            writer.WriteLine(line.Substring(index + md_h3.Length, line.Length - md_h3.Length));
            writer.WriteLine("</h3>");
        } else if (line.StartsWith(md_h4))
        {
            writer.WriteLine("<h4>");
            int index = line.IndexOf(md_h4);
            writer.WriteLine(line.Substring(index + md_h4.Length, line.Length - md_h4.Length));
            writer.WriteLine("</h4>");
        }
        else if (line.Equals("---"))
        {
            writer.WriteLine("<hr>");

        } else if (line.StartsWith("* "))
        {
            if (!inList)
            {
                inList = true;
                writer.WriteLine("<ul>");
            }

            int index = line.IndexOf("* ");
            writer.WriteLine("<li>");
            writer.WriteLine(ProcessText(line.Substring(index + 2, line.Length - 2)));
            writer.WriteLine("</li>");

            if ((inList && i == (lines.Length - 1)) || !lines[i + 1].StartsWith("* "))
            {
                writer.WriteLine("</ul>");
                inList = false;
            }


        }
        else
        {
            if (!String.IsNullOrEmpty(line))
            {
                writer.WriteLine("<p>");
                writer.WriteLine(ProcessText(line));
                writer.WriteLine("</p>");
            }

        }
    }
    writer.WriteLine(foot);
}


static string ProcessText(string input)
{
    if (String.IsNullOrEmpty(input)) return "";

    int begin = input.IndexOf("***");
    if (begin >= 0)
    {
        int end = input.Substring(begin+3).IndexOf("***");
        if (end > 0)
        {
            string formated_text = input.Substring(begin + 3, end);
            string sub = input.Replace("***"+formated_text+"***", "<b><i>"+formated_text+"</i></b>");
            return ProcessText(sub);
        }
    }

    begin = input.IndexOf("**");
    if (begin >= 0)
    {
        int end = input.Substring(begin + 2).IndexOf("**");
        if (end > 0)
        {
            string formated_text = input.Substring(begin + 2, end);
            string sub = input.Replace("**" + formated_text + "**", "<b>" + formated_text + "</b>");
            return ProcessText(sub);
        }
    }

    begin = input.IndexOf("*");
    if (begin >= 0)
    {
        int end = input.Substring(begin + 1).IndexOf("*");
        if (end > 0)
        {
            string formated_text = input.Substring(begin + 1, end);
            string sub = input.Replace("*" + formated_text + "*", "<i>" + formated_text + "</i>");
            return ProcessText(sub);
        }
    }

    begin = input.IndexOf("~~");
    if (begin >= 0)
    {
        int end = input.Substring(begin + 2).IndexOf("~~");
        if (end > 0)
        {
            string formated_text = input.Substring(begin + 2, end);
            string sub = input.Replace("~~" + formated_text + "~~", "<s>" + formated_text + "</s>");
            return ProcessText(sub);
        }
    }

    return input;
}