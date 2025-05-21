//import { event } from 'vue-gtag'

import { helpers, minValue } from "@vuelidate/validators";

class CommonHelper {
  ExportToGrid = (fileName: string, data: any, exportType: number): void => {
    const file = new Blob([data], {
      type:
        Number(exportType ?? 0) == 2
          ? "application/pdf"
          : "application/vnd.ms-excel",
    });
    // process to auto download it
    const fileURL = URL.createObjectURL(file);
    const link = document.createElement("a");
    link.href = fileURL;
    link.download = (file.type || "").includes("/pdf")
      ? `${fileName}.pdf`
      : `${fileName}.xlsx`;
    link.click();
    link.remove();
  };
  setFormInvalidControl = () => {
    setTimeout(() => {
      //(document.querySelector('input.p-invalid, div.p-invalid input, span.p-invalid input, textarea.p-invalid') as HTMLElement).focus();
      const firstInvalid = document.querySelector(
        "input.p-invalid, div.p-invalid input, span.p-invalid input, textarea.p-invalid"
      ) as HTMLElement;
      if (firstInvalid) {
        firstInvalid.focus();
      }
    });
  };
  setFirstControl = (ClasName: string) => {
    setTimeout(() => {
      const firstInvalid = document.querySelector(
        `input.${ClasName}, div.${ClasName} input, span.${ClasName} input, textarea.${ClasName}`
      ) as HTMLElement;
      if (firstInvalid) {
        firstInvalid.focus();
      }
    });
  };
  OpenNewTab = (externalUrl: string, urldetails?: any): void => {
    let Url = externalUrl.replace(/\s/g, "");
    if (
      Url.indexOf("http://") == 0 ||
      Url.indexOf("https://") == 0 ||
      Url.indexOf("ftp") == 0
    ) {
      // do something here
    } else {
      Url = "https://" + Url;
    }
    //event(urldetails, { method: 'Google' });
    window.open(Url);
    // window.open(Url, '_blank', 'toolbar=0,location=0,menubar=0') //For new Window
  };
  convertTwoDecimal = (value: any): string => {
    return parseFloat(value ?? 0).toFixed(2) ?? 0.0;
  };

  numberWithCommas = (x: any): string => {
    return String(x ?? "").split(".")[0].length > 3
      ? String(x ?? "")
        .substring(0, String(x ?? "").split(".")[0].length - 3)
        .replace(/\B(?=(\d{3})+(?!\d))/g, ",") +
      "," +
      String(x ?? "").substring(String(x ?? "").split(".")[0].length - 3)
      : String(x ?? "");
  };
  MinDateValidation = () => {
    let date = new Date("01-01-2000");
    return helpers.withMessage("Invalid date", (value: Date) => value > date);
  }
}
export default new CommonHelper();