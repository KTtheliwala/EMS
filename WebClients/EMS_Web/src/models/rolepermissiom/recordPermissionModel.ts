export class recordPermissionModel{
    RecordPermission! :  RecordPermissionListModel[] | [];
}
export class  RecordPermissionListModel{
    Id !: number | 0;
    RoleId!: number | 0;
    PageId!: number | 0;
    ForAdd!: string | 0;
    ForEdit!: string| 0;
    ForDelete!: string|0;
    ForView!: string|0;
    PageCode!: string | "";    
}
class recordPermissionModelDTO{
    static rules = {}
}
export default recordPermissionModelDTO;