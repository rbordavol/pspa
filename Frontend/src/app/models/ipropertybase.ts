
export interface IPropertyBase {
    id: number;
    sellRent: number;
    propertyName: string;
    propertyType: string;
    furnishingType: string;
    price: number;
    bhk: number;
    builtArea: number;
    city: string;
    readyToMove: boolean;
    possession: Date;
    photo?: string;
}
