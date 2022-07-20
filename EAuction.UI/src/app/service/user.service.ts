import { Injectable } from '@angular/core';
import { map, Observable } from 'rxjs';
import { IUserService } from '../interface/iuser-service';
import { UserModel } from '../model';
import { HttpService } from './http.service';

@Injectable({
  providedIn: 'root'
})
export class UserService implements IUserService {

  constructor(private httpService: HttpService) { }

  public addSeller(user: UserModel): Observable<UserModel> {
    return this.httpService.postSellerService<UserModel>('addseller', user)
      .pipe(
        map(list => {
          return list;
        })
      );
  }

  public addBuyer(user: UserModel): Observable<UserModel> {
    return this.httpService.postBuyerService<UserModel>('addbuyer', user)
      .pipe(
        map(list => {
          return list;
        })
      );
  }

  public viewAllUser(): Observable<UserModel[]> {
    return this.httpService.getSellerService<UserModel[]>('showalluser')
      .pipe(
        map(list => {
          return list;
        })
      );
  }

  public deleteUser(userId: string): Observable<boolean> {
    return this.httpService.deleteSellerService<boolean>('delete-user/' + userId);
  }

  public getUserByID(userId: string): Observable<UserModel> {
    return this.httpService.getSellerService<UserModel>('getuserbyid/' + userId)
      .pipe(
        map(list => {
          return list;
        })
      );
  }

  public getUserByEmail(userEmail: string): Observable<UserModel> {
    return this.httpService.getSellerService<UserModel>('getuserbyemail/' + userEmail)
      .pipe(
        map(list => {
          return list;
        })
      );
  }
}
