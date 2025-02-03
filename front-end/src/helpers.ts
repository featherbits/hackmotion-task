import { environment } from "./environments/environment";

export function analyticsApi(uri: string): string {
    return environment.analyticsSvcUrl + (uri.startsWith('/') ? uri : '/api/' + uri)
}