import os
import re

# this file creates translation json 
# prints it in the console
# or only prints the newly added lines to append the google doc



path = os.path.dirname(__file__)
OUTPUT_FILEPATH = "Translations.cs"

#files to look for strings to replace
FILES = [
    "Player\Main Menu\MainMenu_DifficultySelection.cs",
    "Player\Main Menu\MainMenu.cs",
    "Player\Main Menu\MainMenu_Guide.cs",
    "Player\Main Menu\MainMenu_HUD.cs",
    "Player\Main Menu\MainMenu_Inventory.cs",
    "Player\Main Menu\MainMenu_Perks.cs",
    "Player\Main Menu\MainMenu_Spells.cs",
    "Player\Crafting\Empowering.cs",
    "Player\Crafting\IndividualRerolling.cs",
    "Player\Crafting\Polishing.cs",
    "Player\Crafting\Reforging.cs",
    "Player\Crafting\Rerolling.cs",
    "Player\Perks\PerkDatabase.cs",
    "Player\Spells\SpellDataBase.cs",
    "Items\ItemDataBase_StatDefinitions.cs",
    "Items\ItemDataBase_ItemDefinitions.cs",
    "Items\Item.cs",
    "Res\ResourceLoader.cs",
]

file_variable_cnt = {}
IGNORED_ELEMENTS = ['"P"', '"P0"', '"N"', '"N0"', '"\\n"', '""']
CS_VARIABLE_PREFIX = '\t\tpublic string '
CS_VARIABLE_LEN = len(CS_VARIABLE_PREFIX) + 1
variables_by_str = {}
variables_by_file_groups = {} #dictionary 
#{ filename: pair(number, text) }



if os.path.exists(OUTPUT_FILEPATH):
    output_file = open(OUTPUT_FILEPATH,'r', encoding="utf-8")
    pattern_digits = re.compile(r'\d+')
    for line in output_file.readlines():
        if line.startswith(CS_VARIABLE_PREFIX + "_"):
            variable_name_value_pair = line[CS_VARIABLE_LEN:].split("=")
            v = variable_name_value_pair[0].rstrip()
            k = "=".join(variable_name_value_pair[1:]).strip().rstrip()[:-1]
            v_num_match = pattern_digits.search(v)
            file_name = v[:(v_num_match.start()-1)]
            v_num = int(v[v_num_match.start():])
            if file_name in file_variable_cnt.keys():
                file_variable_cnt[file_name] = max(v_num,file_variable_cnt[file_name])
            else:
                file_variable_cnt[file_name] = v_num
            if file_name in variables_by_file_groups.keys():
                variables_by_file_groups[file_name].append((v_num,k))
            else:
                variables_by_file_groups[file_name] = [(v_num,k)]
            
            variables_by_str[k] = v 
    output_file.close()

out = open(OUTPUT_FILEPATH,'w+', encoding="utf-8")
out.write((
"//Generated with a python script\n"
"namespace ChampionsOfForest.Localization\n"
"{\n"
"\tpublic partial class Translations\n"
"\t{\n"
"\n"
"\n"
))


pattern_format_param = re.compile(r'\{\d+\}')
for k,v in variables_by_file_groups.items():
    v.sort(key=lambda tup: tup[0])
    out.write("\t\t//" + k + "\n")
    for var in v:
        has_format_params = pattern_format_param.match(var[1]) is not None

        privvarName = "_"+ k + "_" + str(var[0])
        if has_format_params:
            out.write('\t\tpublic static string ' + k + "_" + str(var[0]) + "(params object[] objects) => string.Format(instance."+ privvarName +", objects);\n")
        else:
            out.write('\t\tpublic static string ' + k + "_" + str(var[0]) + " => instance."+ privvarName +";\n")
        out.write(CS_VARIABLE_PREFIX + privvarName + " = "+ var[1] +";\n")
        print("Moved old: "+ privvarName + " = "+ var[1])
    out.write("\n")
changec = 0
pattern_string = re.compile(r'"[^"]*"') #find c# strings
pattern_translation_tag = re.compile(r'//tr') #find c# comment that says //tr
pattern_translation_usage = re.compile(r'Translations\.(\w+)(\d)+(\(.+\)(/\*)?)?') #find in c# script if Translation.anything_123 was used
pattern_translation_usage_comment = re.compile(r'/\*.+\*/') # find comment after usage in c#
used_varnames=[]

for file in FILES:
    try:
        p = os.path.join(path, file)
        f = open(p, "r", encoding="utf-8")
        filename = os.path.basename(p).split('.')[0]
        print (file)
        n = file_variable_cnt[filename] + 1 if filename in file_variable_cnt.keys() else 1
        out.write("        //" + file + "\n")
        txt = f.readlines()
        f.close()
        #replace all formattable strings with concat
        for idx, line in enumerate(txt):
            if pattern_translation_tag.search(line):
                strings = reversed(list(pattern_string.finditer(line)))
                for v_num_match in strings:
                    s = v_num_match.group()
                    if s not in IGNORED_ELEMENTS and len(re.sub(r'[\d% \*\.]+', '', s)) > 3:
                        if line[v_num_match.start()-1] == "$":
                            print("Error: found $ before string in " + filename+ ". replacing")
                            s1 = re.sub("{", "\" + (",s)
                            formatting = re.search(r":(\w\d?)}",s1)
                            if formatting:
                                s1 =  s1[:formatting.start()] + ').ToString("' + formatting.group(1) + '") + \"' + s1[formatting.end():]
                            s1 = re.sub("}", ") + \"",s1)
                            txt[idx] = line[:v_num_match.start()-1] + s1 +  line[v_num_match.end():]
            
        # find new variables and append translations.cs with them
        for idx, line in enumerate(txt):
            if pattern_translation_tag.search(line):
                strings = reversed(list(pattern_string.finditer(line)))
                
                for v_num_match in strings:
                    s = v_num_match.group()
                    if s not in IGNORED_ELEMENTS and len(re.sub(r'[\d% \*\.]+', '', s)) > 3:
                        if s not in variables_by_str.keys():
                            print("Found translateable string in " + line)
                            varname = filename + "_" + str(n)
                            has_format_params = pattern_format_param.match(var[1]) is not None

                            out.write('\t\tpublic static string ' + varname + " => instance._"+ varname +";\n")
                            out.write(CS_VARIABLE_PREFIX + "_" + varname + " = "+ s +";\n\n")
                            variables_by_str[s] = varname
                            if filename in variables_by_file_groups.keys():
                                variables_by_file_groups[filename].append((n,s))
                            else:
                                variables_by_file_groups[filename] = [(n,s)]
                            n += 1
                            changec += 1

        # replace all const string occurences in the files
        for idx, line in enumerate(txt):
            if pattern_translation_tag.search(line):
                strings = reversed(list(pattern_string.finditer(line)))
                for v_num_match in strings:
                    s= v_num_match.group()
                    if s not in IGNORED_ELEMENTS and len(s) > 3:
                        if s in variables_by_str.keys():
                            txt[idx] = line[:v_num_match.start()] + "Translations." + variables_by_str[s] + '' + line[v_num_match.end():] 
        

        # add a comment after every usage of Translation 
        for idx, line in enumerate(txt):
            if pattern_translation_usage.search(line) is not None:
                strings = reversed(list(pattern_translation_usage.finditer(line)))
                for v_num_match in strings:
                    s= v_num_match.group()
                    varname=  re.search(r'(?<=Translations\.)\w+\d+', s).group()
                    used_varnames.append(varname)
                    keys = [k for k, v in variables_by_str.items() if v == varname]
                    if len(keys) > 0:
                        comment_end = pattern_translation_usage_comment.search(line).end()
                        txt[idx] = line[:v_num_match.end()] + "/* " + keys[0] + " */" +line[comment_end:] 
                    else:
                        print("cant update text in line \n"+ line + f"\nvarname={varname}, s={s}, k={keys}")

     
        out.write("\n")
        f = open(p,"w",encoding="utf-8")
        f.writelines(txt)
        f.close()
    
    except Exception as e:
        print(str(e))

out.write((
"\n\n"
"\t}\n"
"}\n\n\n/* for translation cpy paste:\n\n"
))
for varname in variables_by_str.values():
    if varname not in used_varnames:
        print(varname + " is never used")

for k,v in variables_by_file_groups.items():
    v.sort(key=lambda tup: tup[0])
    for var in v:
        print(k+ "_" + str(var[0]) +':: '+ var[1])
        out.write(k+ "_" + str(var[0]) +':: '+ var[1] +"\n" )
out.write('\n\n\n*/')
out.close()
print("Translations generated")