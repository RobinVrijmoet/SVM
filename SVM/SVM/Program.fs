open System
open System.IO
open SVMAST
open ParserUtils
open SVM
open Microsoft.FSharp.Text.Lexing

let rec virtualmachine((parsedAST : Instruction list),( memorysize : int))=
    //printfn "%A" parsedAST.Head
    //printfn " hello"
    //match parsedAST.Head with 
    //| Mov( x,y,z)-> printfn " TESTING  THIS IS A MOV"
    //| _ -> ()
    //if(match parsedAST.Head with |Mov) then printfn "%A" parsedAST.Head
    let memory = List<SVMAST.Literal> 
    if not parsedAST.Tail.IsEmpty then virtualmachine(parsedAST.Tail,memorysize)


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
