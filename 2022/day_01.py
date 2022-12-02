# Day 1: Calorie Counting

with open("puzzles/day_01.txt") as file:
    input = file.read()

elvesCalories = list(map(lambda e: sum(map(int, e.split("\n"))), input.split("\n\n")))
elvesCalories.sort(reverse=True)
print("Part 1: ", elvesCalories[0])
print("Part 2: ", sum(elvesCalories[:3]))