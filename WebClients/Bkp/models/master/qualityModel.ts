export class qualityModel{
    Id!: number | 0;  
    Name!: string | "";
    ShortName!: string | "";
    GroupId!: number | 0;
    SubGroupId!: number | 0;
    HSNCode!: string | "";
    Denier!: number |0;
    Filament!: number | 0 ;
    Luster!: string | "";
    QualityType!: string | "";
    Description!: string | "";
    SortOrder!: number | 0;
    IsActive!: boolean |true;
  }

import { required, helpers } from '@vuelidate/validators'
class qualityModelDTO {
    static model:qualityModel=new qualityModel();
    static rules = { 
        Name: { required :helpers.withMessage('Name is required', required)}
      }
}
export default qualityModelDTO