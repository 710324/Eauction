import { Observable } from "rxjs";
import { BuyerBidModel, ProductBidsModel, ProductModel, ProductToBuyer } from "../model";

export abstract class IProductService {

    public abstract addProduct(product: ProductModel): Observable<ProductModel>;

    public abstract viewAllProducts(): Observable<ProductModel[]>;

    public abstract viewAllProduct(productId: string | null): Observable<ProductModel | undefined>;

    public abstract viewAllBidForProduct(productId: string): Observable<ProductBidsModel>;

    public abstract placeBid(data: BuyerBidModel): Observable<ProductToBuyer>;

    public abstract deleteProduct(data: string): Observable<boolean>;

    public abstract getBidForProduct(productBidId: string | null): Observable<BuyerBidModel>;

    public abstract updateBid(data: BuyerBidModel): Observable<ProductToBuyer>
}
