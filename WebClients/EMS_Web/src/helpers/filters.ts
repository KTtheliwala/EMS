import { format } from "crypto-js";
import moment from "moment";

const filters = {
    price(input: any) {
      if (!isNaN(parseFloat(input))) {
        return parseFloat(input).toFixed(2);
      }
      return '';
    }
  }
  export default filters;
  