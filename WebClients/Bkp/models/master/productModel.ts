export class productModel{
    Id!: number | 0;  
    CategoryId!: number | 0;
    ProductCode  !: string | "";
    Name!:string|""; 
    PlainTextName!:string|""; 
    Description!: string | "";
    ShortDescription!: string | "";
    Price!: number | 0;  
    OfferText!: string | '';
    IsOfferApplicable!:boolean|false;
    TopSellingProduct!:boolean|false;
    IsNewProduct!:boolean|false;
    Rating!: string | '';
    MetaTitle!: string | "";  
    MetaKeyword!: string | "";  
    MetaDescription!: string | "";  
    IsActive!:boolean|false;
    SortOrder!: number | 0;
}

export class productImageModel {
    Id!: number | 0; 
    ProductId!: number | 0;   
    OriginalFileName!:string|""; 
    FileName!:string|""; 
    AltTitle!:string|""; 
    AltKeyword!:string|""; 
    AltDescription!:string|""; 
    SortOrder!: number | 0;
    IsThumbnail!: boolean | false;
    file!: File | null;

}
import { required, helpers } from '@vuelidate/validators'
class productModelDTO {
    static model:productModel=new productModel();
    static rules = { 
        Name: { required :helpers.withMessage('Name is required', required)},
        ProductCode: { required :helpers.withMessage('Product code is required', required)},
        CategoryId:{ required :helpers.withMessage('Category is required', required)},
      }
}
export default productModelDTO