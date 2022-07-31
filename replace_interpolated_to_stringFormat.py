import os
import re

# this file converts all interpolated strings in a file to string.Format


path = os.path.dirname(__file__)

#files to look for strings to replace
files = [
    # "Player\Main Menu\MainMenu_DifficultySelection.cs",
    # "Player\Main Menu\MainMenu.cs",
    # "Player\Main Menu\MainMenu_Guide.cs",
    # "Player\Main Menu\MainMenu_HUD.cs",
    # "Player\Main Menu\MainMenu_Inventory.cs",
    # "Player\Main Menu\MainMenu_Perks.cs",
    # "Player\Main Menu\MainMenu_Spells.cs",
    # "Player\Crafting\Empowering.cs",
    # "Player\Crafting\IndividualRerolling.cs",
    # "Player\Crafting\Polishing.cs",
    # "Player\Crafting\Reforging.cs",
    # "Player\Crafting\Rerolling.cs",
     "Player\Perks\PerkDatabase.cs",
     "Player\Spells\SpellDataBase.cs",
    # "Items\ItemDataBase_StatDefinitions.cs",
    # "Items\ItemDataBase_ItemDefinitions.cs",
    # "Items\Item.cs",
    "Res\ResourceLoader.cs",
]

ignore = ['"P"', '"P0"', '"N"', '"N0"', '"\\n"', '""']

pattern2 = re.compile(r'(?<=\$)"([^"]|((?<=\\)"))+"') #find strings that start with $ with \" characters included
pattern3 = re.compile(r'(\{[^}]+\})') # find curly brackets and its content

for file in files:

    p = os.path.join(path, file)
    f = open(p, "r", encoding="utf-8")
    lines = f.readlines()
    f.close()
    for idx, line in enumerate(lines):
        if(pattern2.search(line)):
            print("replacing $" + line)
            format_params = ""
            brackets = list(pattern3.finditer(line))
            n = len(brackets) - 1

            rev_brackets = reversed(brackets)
            for bracket in rev_brackets:
                content = bracket.group()[1:-1]
                format_params = ", " +  content + format_params
                line = line[:bracket.start()] + '{'+str(n)+'}' + line[bracket.end():]
                n-=1
            match2 = pattern2.search(line)
            line = line[:match2.start()-1]  +"string.Format(" + match2.group() + format_params+ ")" +  line[match2.end():]
            print("with "+ line+ "\n")
            lines[idx] = line
                
    f = open(p,"w",encoding="utf-8")
    f.writelines(lines)
    f.close()