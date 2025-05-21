export class ordersModel{
    Id!: number | 0;
    OrderNo!: number | 0;
    CompanyId!: number | 0;
    CompanyName!: string | "";
    DateofDelivery!: string | null;
    DateOfFunction!: string | null; 
    ContactName!: string | "";
    ContactNumber!: string | "";
    ContactEmail!: string | "";
    DeliveryAddress!: string | "";
    DeliveryAddress2!: string | "";
    City!: string | "";
    State!: string | "";
    PostCode!: string | "";
    CountryId!: number | null;
    CountryName!: string | "";
    OrderStatus!: number | 0;
    OrderStatusName!: string | "";
    OrderDetails!: OrderDetailsModel | [];
    //DiscountTypeId!:number | null;
    DiscountAmount!:number | 0;
    TotalAmount!: number | 0;
    SubTotalAmount!: number | 0;
    Remarks!: string | "";
}

export class OrderDetailsModel {
    SrNo!: number | 0;
    Id!: number | 0;
    OrderId!: number | 0;
    ProductId!: number | 0;
    ProductName!: string | "";
    ProductCode!: string | "";
    Remarks!: string | "";
    Quantity!: number | 0;  
    ProductPrice!: number | 0;  
    TotalAmount!: number | 0;  
}

export class OrderStatusChange {
    Id!: number | 0;
    OrderId!: number | 0;
    OrderStatus!: number | 0;
    Remarks!: string|"";
}

import { required, helpers, maxLength, minLength, email } from '@vuelidate/validators'
class ordersModelDTO {
    static model:ordersModel=new ordersModel();
    static rules = { 
        CompanyId: { required: helpers.withMessage('Company name is required', required) },       
        DateofDelivery: { required :helpers.withMessage('Date Of Delivery is required', required)},
        DeliveryAddress: { required :helpers.withMessage('Delivery address is required', required)},
        City: { required :helpers.withMessage('City is required', required)},
        //State: { required :helpers.withMessage('State is required', required)},
        PostCode: { required :helpers.withMessage('Postal code is required', required)},
        CountryId: { required :helpers.withMessage('Country is required', required)},
        DateOfFunction: { required :helpers.withMessage('Date Of Function is required', required)},
        ContactName: { required :helpers.withMessage('Contact Name is required', required)},
        ContactEmail: { required: helpers.withMessage('Contact Email is required', required), email: helpers.withMessage("Invalid Email", email) },
      }

      static statusRule = {
        OrderStatus: { required: helpers.withMessage('Order status is required', required) },   
      }
}

export default ordersModelDTO