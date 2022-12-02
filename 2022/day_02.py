# Day 2: Rock Paper Scissors

strategyGuide1 = {
    "A X": 4, "A Y": 8, "A Z": 3,
    "B X": 1, "B Y": 5, "B Z": 9,
    "C X": 7, "C Y": 2, "C Z": 6,
}

strategyGuide2 = {
    "A X": 3, "A Y": 4, "A Z": 8,
    "B X": 1, "B Y": 5, "B Z": 9,
    "C X": 2, "C Y": 6, "C Z": 7,
}

with open("puzzles/day_02.txt") as file:
    input = file.read().splitlines()

print("Part 1: ", sum(map(lambda i: strategyGuide1[i], input)))
print("Part 2: ", sum(map(lambda i: strategyGuide2[i], input)))