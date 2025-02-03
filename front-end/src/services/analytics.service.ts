import { HttpClient } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import { BehaviorSubject, EMPTY, filter, Observable, switchMap, tap } from 'rxjs';
import { analyticsApi } from '../helpers';
import { PLATFORM_ID } from '@angular/core';
import { isPlatformBrowser } from '@angular/common';

interface CreateAnalyticsUserResponse {
  token: string
}

const tokenStoreKey = 'AnalyticsService.token'

export enum AnalyticsEvent {
  PageView = 'PageView',
  VideoCompleted = 'VideoCompleted'
}

@Injectable({
  providedIn: 'root'
})
export class AnalyticsService {

  private readonly token = new BehaviorSubject<null | string>(null)
  private readonly inBrowser: boolean

  constructor(
    private http: HttpClient,
    @Inject(PLATFORM_ID) platformId: Object
  ) {
    this.inBrowser = isPlatformBrowser(platformId)
  }

  private getToken(): Observable<string> {
    return this.token.pipe(tap(token => {
      if (!token) {
        token = localStorage.getItem(tokenStoreKey)
        if (token) {
          this.token.next(token)
        } else {
          this.http.post<CreateAnalyticsUserResponse>(analyticsApi('analytics/user'), {
            screenWidth: window.screen.height,
            screenHeight: window.screen.width,
            userAgent: navigator.userAgent
          }).subscribe(result => {
            this.token.next(result.token)
          })
        }
      }
    })).pipe(filter((token): token is string => !!token))
  }

  public logEvent(name: string): Observable<void> {
    return this.inBrowser ? this.getToken().pipe(switchMap(token => this.http.post<void>(analyticsApi('analytics/event'), {
      token, name, pageUrl: window.location.href
    }))) : EMPTY
  }
}
