import http from 'k6/http';
import { sleep } from 'k6';

// Configuration options for the load test.
export const options = {
    duration: '1m',
    vus: 50,
    thresholds: {
        http_req_failed: ['rate<0.01'],
        http_req_duration: ['p(95)<500']
    },
}

// Script for load test:
export default function() {
    //const res = http.get('http://10.136.0.115:80/movies-service/api/Movies')
    const res = http.get('http://192.168.240.6:30518/agenda/GetOne/026a5bc6-a711-4ee2-9a09-5b64ed763871s');
    sleep(1);
}
