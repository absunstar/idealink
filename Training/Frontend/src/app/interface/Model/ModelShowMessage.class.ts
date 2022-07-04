export class ShowMessage
{
    type : EnumShowMessage ;
    message: string;
    Error(msg : string):void{
        this.type = EnumShowMessage.Error ;
        this.message= msg;
    }
    Success(msg : string):void{
        this.type = EnumShowMessage.Success;
        this.message= msg;
    }
    Send(msg:string, isSucess:boolean)
    {
        this.type = isSucess? EnumShowMessage.Success : EnumShowMessage.Error;
        this.message= msg;
    }
}
export enum EnumShowMessage
{
    Success = 1,
    Error = 2
}