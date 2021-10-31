import os
import fnmatch
import re
import io
path = os.path.dirname(__file__)
outputfilepath = "Translations.cs"
while(True):
    Repeat = False
    #files to look for strings to replace
    files = [
        # "Player\Main Menu\MainMenu.cs",
        "Player\Main Menu\MainMenu_DifficultySelection.cs",
        "Player\Main Menu\MainMenu_Guide.cs",
        # "Player\Main Menu\MainMenu_HUD.cs",
        # "Player\Main Menu\MainMenu_Inventory.cs",
        # "Player\Main Menu\MainMenu_Perks.cs",
        # "Player\Main Menu\MainMenu_Spells.cs",
    ]

    # recursive way to find cs files
    # matches = []
    # for root, dirnames, filenames in os.walk(os.getcwd()):
    #     for filename in fnmatch.filter(filenames, '*.cs'):
    #         matches.append(os.path.join(root, filename))


    file_variable_cnt = {}
    ignore = ['"P"', '"P0"', '"N"', '"N0"', '"\\n"', '""']
    var_pre = '        public static string '
    var_pre_len = len(var_pre)
    outread = open(outputfilepath,'r', encoding="utf-8")
    variables = {}
    vargroups = {}
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

    out = open(outputfilepath,'w', encoding="utf-8")
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
            strings = pattern2.findall(txt)
            for s in strings:
                if s not in ignore and len(s) > 3:
                    if s not in variables.keys():
                        varname = filename + "_" + str(n)
                        print(varname +": "+s)
                        n += 1
                        out.write(var_pre + varname + " = "+ s +";\n")
                        variables[s] = varname
                        changec += 1
            # replace all text in the files
            strings = reversed(list(pattern2.finditer(txt)))
            for match in strings:
                s= match.group()
                if s not in ignore and len(s) > 3:
                    if s in variables.keys():
                        if txt[match.start()-1] == "$":
                            print("Error: found $ before string in " + filename+ "replacing")
                            s = re.sub("{", "\"+ (",s)
                            s = re.sub("}", ")+\"",s)
                            txt[:match.start()] + s +  txt[match.end():]
                            Repeat = True;
                            continue
                        txt =  txt[:match.start()] + "Translations." + variables[s] + '/*' + s + '*/' + txt[match.end():] 
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
    if Repeat:
        os.remove(outputfilepath)
    else:
        if changec == 0:
            print('{')
            for k,v in vargroups.items():
                v.sort(key=lambda tup: tup[0])
                for var in v:
                    print('\t"' +k+ "_" + str(var[0]) +'": '+ var[1] +"," )
            print('}')
        break
