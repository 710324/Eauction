import { Observable } from "rxjs";
import { UserModel } from "../model/user.model";


export abstract class IUserService {
    public abstract addSeller(product: UserModel): Observable<UserModel>;
    public abstract addBuyer(product: UserModel): Observable<UserModel>;
    public abstract viewAllUser(): Observable<UserModel[]>;
    public abstract deleteUser(userId: string): Observable<boolean>;

    public abstract getUserByID(userId: string): Observable<UserModel>;
    public abstract getUserByEmail(userEmail: string): Observable<UserModel>;
}
