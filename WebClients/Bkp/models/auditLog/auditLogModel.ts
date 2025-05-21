export class AuditLogModel{
    ActionId: number = 0; 
    Action:string =""; 
    Entity:string="";
    EntityId: number = 0;      
    Message:string="";
    Level:string="";
    Exception:string="";
    Properties:string="";
    LogTypeId: number = 0;     
    LogType:string="";
    LogSourceId: number = 0;           
    TimeStamp:Date | undefined;
    UserId: number = 0;   
    Username:string="";
}

export default AuditLogModel