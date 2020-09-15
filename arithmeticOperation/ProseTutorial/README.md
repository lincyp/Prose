Open arithmeticOperation/prose.sln

Program.cs: run this file to give example, interactive window

Synthesis/Semantics.cs: Operator definitions

Synthesis/grammar/arithmetic.grammar: Grammar file, runtime path:bin/Debug/netcoreapp2.1/...

Synthesis/WitnessFunctions: Backpropagation learner

Operators: ElementAt(list, index), Add(a, b), Multiply(a,b)

Note: 1. Ranking is currently not used, Ranking.cs is just a placeholder. 
      2. Since features/ranking not used here, giving more than 1 inputs might not yield desired program at least for complex operations
      3. Ensure any grammar changes made get reflected in bin/Debug/netcoreapp2.1/Synthesis/grammar/arithmetic.grammar


TODO: Include divide operation
