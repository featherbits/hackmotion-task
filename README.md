# hackmotion-task

1. Create `.env` from `.env.example` by executing `cp .env.example .env` and update values accordingly if you are not pleased with defaults
2. Run `docker compose build` and then `docker compose up`
3. Navigate your browser to default URL `http://localhost:4000` or update port value from `FrontEndPort` in `.env` if defaults has been changed

Analytics events are logged to the console of `hackmotion-analytics` service (`docker compose logs -f  analytics`)
