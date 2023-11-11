import { BookModel } from "./book.model"

export class PaymentModel {
    userId: number = 0;
    books: BookModel[] = []
    buyer: BuyerModel = new BuyerModel();
    shippingAddress: AddressModel = new AddressModel();
    billingAddress: AddressModel = new AddressModel();
    paymentCard: PaymentCardModel = new PaymentCardModel();
}

export class BuyerModel {
    id: string = "";
    name: string = "Oğuzhan";
    surname: string = "Muratoğlu";
    identityNumber: string = "11111111111";
    email: string = "oguzhan@gmail.com";
    gsmNumber: string = "5549528006";
    registrationDate: string = "";
    lastLoginDate: string = "";
    registrationAddress: string = "";
    city: string = "";
    country: string = "";
    zipCode: string = "";
    ip: string = "";
}

export class AddressModel {
    description: string = "BOLU";
    zipCode: string = "38070";
    contactName: string = "Oğuzhan Muratoğlu";
    city: string = "BOLU";
    country: string = "Turkey";
}

export class PaymentCardModel {
    cardHolderName: string = "Oğuzhan Muratoğlu";
    cardNumber: string = "";
    expireYear: string = "";
    expireMonth: string = "";
    cvc: string = "377";
}