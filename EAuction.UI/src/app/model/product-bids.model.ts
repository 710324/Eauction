import { BidDetailModel } from "./bid-detail.model";

export class ProductBidsModel {

    productName: string;
    shortDescription: string;
    detailedDeceription: string;
    category: string;
    bidEndDate: string;
    startingPrice: number;
    bidDetails: BidDetailModel[];
}
