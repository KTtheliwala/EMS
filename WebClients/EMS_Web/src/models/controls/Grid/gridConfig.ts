import SearchGrid from "./searchGrid";

export default interface GridConfig {
        sortcolumn: string;
        sortorder: number;
        filters: SearchGrid[];
        api: string;
        deletePopup?: Object;
        updatePopup?: Object;
        deleteApi:string;
        doubleClickHander:Function;
        //order ?: GridOrder[]
}
