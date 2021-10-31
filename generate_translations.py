import os
import re

# this file creates translation json 
# prints it in the console
# or only prints the newly added lines to append the google doc



path = os.path.dirname(__file__)
outputfilepath = "Translations.cs"
jsonPath = "obj\\translations_output.json"
jsonInfo = open(os.path.join(path,jsonPath),"w+", encoding='utf-8')
#files to look for strings to replace
files = [
    "Player\Main Menu\MainMenu.cs",
    "Player\Main Menu\MainMenu_DifficultySelection.cs",
    "Player\Main Menu\MainMenu_Guide.cs",
    "Player\Main Menu\MainMenu_HUD.cs",
    "Player\Main Menu\MainMenu_Inventory.cs",
    "Player\Main Menu\MainMenu_Perks.cs",
    "Player\Main Menu\MainMenu_Spells.cs",
    # "Player\Crafting\Empowering.cs",
    # "Player\Crafting\IndividualRerolling.cs",
    # "Player\Crafting\Polishing.cs",
    # "Player\Crafting\Reforging.cs",
    # "Player\Crafting\Rerolling.cs",
    # "Player\Perks\PerkDatabase.cs",
    # "Player\Spells\SpellDataBase.cs",
    # "Items\ItemDataBase_StatDefinitions.cs",
    # "Items\ItemDataBase_ItemDefinitions.cs",
]

file_variable_cnt = {}
ignore = ['"P"', '"P0"', '"N"', '"N0"', '"\\n"', '""']
var_pre = '        public static string '
var_pre_len = len(var_pre)
variables = {}
vargroups = {}
if os.path.exists(outputfilepath):
    outread = open(outputfilepath,'r', encoding="utf-8")
    pattern1 = re.compile(r'\d+')
    for line in outread.readlines():
        if line.startswith(var_pre):
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
"namespace ChampionsOfForest\n"
"{\n"
"	public static class Translations\n"
"	{\n"
"\n"
"\n"
))
for k,v in vargroups.items():
    v.sort(key=lambda tup: tup[0])
    out.write("        //" + k + "\n")
    for var in v:
        out.write(var_pre + k+ "_" + str(var[0]) + " = "+ var[1] +";\n" )
    out.write("\n")
changec = 0
pattern2 = re.compile(r'"[^"]*"')
for file in files:
    # try:
        p = os.path.join(path, file)
        f = open(p, "r", encoding="utf-8")
        filename = os.path.basename(p).split('.')[0]
        n = file_variable_cnt[filename] + 1 if filename in file_variable_cnt.keys() else 1
        out.write("        //" + file + "\n")
        txt = f.read()
        f.close()

        #replace all formattable strings with concat
        strings = reversed(list(pattern2.finditer(txt)))
        for match in strings:
            s= match.group()
            if s not in ignore and len(s) > 3:
                if txt[match.start()-1] == "$":
                    print("Error: found $ before string in " + filename+ ". replacing")
                    s1 = re.sub("{", "\" + (",s)
                    formatting = re.search(r":(\w\d?)}",s1)
                    if formatting:
                        s1 =  s1[:formatting.start()] + ').ToString("' + formatting.group(1) + '") + \"' + s1[formatting.end():]
                    s1 = re.sub("}", ") + \"",s1)
                    txt = txt[:match.start()-1] + s1 +  txt[match.end():]
        
        # find new variables and append translations.cs with them
        strings = pattern2.findall(txt)
        for s in strings:
                if s not in ignore and len(s) > 3:
                    if s not in variables.keys():
                        varname = filename + "_" + str(n)
                        jsonInfo.write('\t"' + varname +'": '+s + ",\n")
                        n += 1
                        out.write(var_pre + varname + " = "+ s +";\n")
                        variables[s] = varname
                        changec += 1

        # replace all const string occurences in the files
        strings = reversed(list(pattern2.finditer(txt)))
        for match in strings:
            s= match.group()
            if s not in ignore and len(s) > 3:
                if s in variables.keys():
                    txt =  txt[:match.start()] + "Translations." + variables[s] + '/*og:' + s[1:-1] + '*/' + txt[match.end():] 
        out.write("\n")
        f = open(p,"w",encoding="utf-8")
        f.write(txt)
        f.close()
    
    # except Exception as e:
    #     print()

out.write((
"\n\n"
"   }\n"
"}\n"
))
out.close()

if changec == 0:
    print('complete json')
    jsonInfo.write('{\n')
    for k,v in vargroups.items():
        v.sort(key=lambda tup: tup[0])
        for var in v:
            jsonInfo.write('\t"' +k+ "_" + str(var[0]) +'": '+ var[1] +",\n" )
    jsonInfo.write('}\n')
else:
    print("only updates noted to json")
jsonInfo.close()
