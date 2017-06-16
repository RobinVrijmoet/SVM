open System
open System.IO
open SVMAST
open ParserUtils
open SVM
open Microsoft.FSharp.Text.Lexing

//Every instruction is handle in here: 
let handle(current : Instruction, memory:array<Literal>)=
    match current with
    | Mov( x,y,z)-> printfn " TESTING  THIS IS A MOV"
    | Nop(x)-> printfn "..."
    | And( x,y,z)-> printfn "and.."
    | _ -> ()


//(recursive function that analyses the list line by line(programm counter->instructionlist)
let rec virtualmachine(parsedAST : Instruction list,memorysize : int,memory:array<Literal>,pc : int)=
    
    handle(parsedAST.[pc],memory)
    if not parsedAST.Tail.IsEmpty then virtualmachine(parsedAST.Tail,memorysize,memory,pc+1)




    
//(set memory etc??)
let initiate((parsedAST : Instruction list),( memorysize : int))=
    //let memory =  [ for i in 1 .. memorysize -> memorysize @ Literal.Address ]
    
    let memory : Literal[] = Array.zeroCreate memorysize
    virtualmachine(parsedAST,memorysize,memory,0)



let parseFile (fileName : string) =
  let inputChannel = new StreamReader(fileName)
  let lexbuf = LexBuffer<char>.FromTextReader inputChannel
  let parsedAST = Parser.start Lexer.tokenstream lexbuf
  

  parsedAST
  

[<EntryPoint>]
let main argv =
  try
    if argv.Length = 2 then
      let ast = parseFile argv.[0]
      
      do printfn "%A" ast 
      virtualmachine(ast,argv.[1]|>int)
      0
    else
      do printfn "You must specify a command line argument containing the path to the program source file and the size of the memory"
      1
  with
  | ParseError(msg,row,col) ->
      do printfn "Parse Error: %s at line %d column %d" msg row col
      1
  | :? Exception as e ->
      do printfn "%s" e.Message
      1
