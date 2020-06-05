namespace FileSplit.Core.Types

open System.Threading.Tasks
open System.IO


type IFolderPicked =
    abstract member FolderWasPicked : unit -> bool
    abstract member GetFolderPath : unit -> string
    abstract member CreateFile : string -> Task<Stream> 
    abstract member OpenInExplorer : unit -> Task
    
type IFolderPickService =
    abstract member PickFolder : unit -> Task<IFolderPicked>
