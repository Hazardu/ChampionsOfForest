import os
import re

# this file creates translation json 
# prints it in the console
# or only prints the newly added lines to append the google doc



path = os.path.dirname(__file__)
outputfilepath = "Translations.cs"
#files to look for strings to replace
files = [
    "Player\Main Menu\MainMenu_DifficultySelection.cs",
    # "Player\Main Menu\MainMenu.cs",
    # "Player\Main Menu\MainMenu_Guide.cs",
    # "Player\Main Menu\MainMenu_HUD.cs",
    "Player\Main Menu\MainMenu_Inventory.cs",
    # "Player\Main Menu\MainMenu_Perks.cs",
    # "Player\Main Menu\MainMenu_Spells.cs",
    # "Player\Crafting\Empowering.cs",
    # "Player\Crafting\IndividualRerolling.cs",
    # "Player\Crafting\Polishing.cs",
    # "Player\Crafting\Reforging.cs",
    # "Player\Crafting\Rerolling.cs",
    # "Player\Perks\PerkDatabase.cs",
    # "Player\Spells\SpellDataBase.cs",
    # "Items\ItemDataBase_StatDefinitions.cs",
    # "Items\ItemDataBase_ItemDefinitions.cs",
    "Items\Item.cs",
]

file_variable_cnt = {}
ignore = ['"P"', '"P0"', '"N"', '"N0"', '"\\n"', '""']
var_pre = '        private string '
var_pre_len = len(var_pre) + 1
variables = {}
vargroups = {}
if os.path.exists(outputfilepath):
    outread = open(outputfilepath,'r', encoding="utf-8")
    pattern1 = re.compile(r'\d+')
    for line in outread.readlines():
        if line.startswith(var_pre + "_"):
            split_var = line[var_pre_len:].split("=")
            v = split_var[0].rstrip()
            k = split_var[1].strip().rstrip()[:-1]
            m = pattern1.search(v)
            fname = v[:(m.start()-1)]
            intval = int(v[m.start():])
            if fname in file_variable_cnt.keys():
                file_variable_cnt[fname] = max(intval,file_variable_cnt[fname])
            else:
                file_variable_cnt[fname] = intval
            if fname in vargroups.keys():
                vargroups[fname].append((intval,k))
            else:
                vargroups[fname] = [(intval,k)]
            
            variables[k] = v 
    outread.close()

out = open(outputfilepath,'w+', encoding="utf-8")
out.write((
"namespace ChampionsOfForest.Localization\n"
"{\n"
"	public partial class Translations\n"
"	{\n"
"\n"
"\n"
))
for k,v in vargroups.items():
    v.sort(key=lambda tup: tup[0])
    out.write("        //" + k + "\n")
    for var in v:
        privvarName = "_"+ k + "_" + str(var[0])
        out.write('        public static string ' + k + "_" + str(var[0]) + " => instance."+ privvarName +";\n")
        out.write('        public string ' + k + "__" + str(var[0]) + " {get => instance."+ privvarName +"; set {instance."+ privvarName + " = value;} }\n")
        out.write(var_pre + privvarName + " = "+ var[1] +";\n")
    out.write("\n")
changec = 0
pattern2 = re.compile(r'"[^"]*"')
pattern3 = re.compile(r'//tr')

for file in files:
    # try:
        p = os.path.join(path, file)
        f = open(p, "r", encoding="utf-8")
        filename = os.path.basename(p).split('.')[0]
        n = file_variable_cnt[filename] + 1 if filename in file_variable_cnt.keys() else 1
        out.write("        //" + file + "\n")
        txt = f.readlines()
        f.close()
        #replace all formattable strings with concat
        for idx, line in enumerate(txt):
            if pattern3.search(line):
                strings = reversed(list(pattern2.finditer(line)))
                for match in strings:
                    s = match.group()
                    if s not in ignore and len(s) > 3:
                        if line[match.start()-1] == "$":
                            print("Error: found $ before string in " + filename+ ". replacing")
                            s1 = re.sub("{", "\" + (",s)
                            formatting = re.search(r":(\w\d?)}",s1)
                            if formatting:
                                s1 =  s1[:formatting.start()] + ').ToString("' + formatting.group(1) + '") + \"' + s1[formatting.end():]
                            s1 = re.sub("}", ") + \"",s1)
                            txt[idx] = line[:match.start()-1] + s1 +  line[match.end():]
            
        # find new variables and append translations.cs with them
        for idx, line in enumerate(txt):
            if pattern3.search(line):
                strings = reversed(list(pattern2.finditer(line)))
                for match in strings:
                    s = match.group()
                    if s not in ignore and len(s) > 3:
                        if s not in variables.keys():
                            varname = filename + "_" + str(n)
                            n += 1

                            out.write('        public static string ' + varname + " => instance._"+ varname +";\n")
                            out.write('        public string ' + k + "__" + str(n) + " {get => instance._"+ varname +"; set {instance._"+ varname + " = value;} }\n")
                            out.write(var_pre + "_" + varname + " = "+ s +";\n\n")
                            variables[s] = varname
                            changec += 1

        # replace all const string occurences in the files
        for idx, line in enumerate(txt):
            if pattern3.search(line):
                strings = reversed(list(pattern2.finditer(line)))
                for match in strings:
                    s= match.group()
                    if s not in ignore and len(s) > 3:
                        if s in variables.keys():
                            txt[idx] = line[:match.start()] + "Translations." + variables[s] + '/*og:' + s[1:-1] + '*/' + line[match.end():] 
        out.write("\n")
        f = open(p,"w",encoding="utf-8")
        f.writelines(txt)
        f.close()
    
    # except Exception as e:
    #     print()

out.write((
"\n\n"
"   }\n"
"}\n\n\n/* json:\n\n"
))

out.write('{\n')
for k,v in vargroups.items():
    v.sort(key=lambda tup: tup[0])
    for var in v:
        out.write('\t"' +k+ "_" + str(var[0]) +'": '+ var[1] +",\n" )
out.write('}\n\n\n*/')
out.close()
print("Translations generated")