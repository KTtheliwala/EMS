import SearchGrid from "@/models/controls/Grid/searchGrid";
// import ExportGrid from "@/models/controls/exportGrid";
import GridColumn from "@/models/controls/Grid/gridColumn";
export default interface GridRequest {
        first: number;
        rows: number;
        sortField: string;
        sortOrder: number;
        filters: SearchGrid[];
        //exportFilters: ExportGrid[];
        columns: GridColumn[];
        //globalFilterColumns: string,
        //globalFilterText: string,
        exportType?: ExportType;
        ResponseType?: ExportType;
        Length?: number;
        Timezone:string
}

export enum ExportType {
        Excel = 1,
        Pdf = 2,
        None = 0,
}
