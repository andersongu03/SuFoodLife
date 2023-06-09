const homePage = new Vue({
    el: '#homePage',
    data: {
        homePageArr: [],
        CarouselArr: [],
        AnnouncementArr: [],
        StarComment:[],
        HotSaleArr: [],
        keyword:"",
    },
    methods: {
        //取得評論資料
        GetStarCommentDetail() {
            let _this = this;
            axios.get("/Index/GetStarInfo").then(response => {
                _this.StarComment = response.data;
                let RList = []
                for (var i = 0; i < _this.StarComment.length; i++) {
                    var item = _this.StarComment[i];
                    RList.push(item);
                }
                _this.StarComment = RList
            })
        },
    //取得首頁資料
    GethomePageDetail() {
        let _this = this;
            axios.get("/Index/GetHomePageInfo").then(response => {
                _this.homePageArr = response.data;
            let AList = []
                for (var i = 0; i < _this.homePageArr.length; i++) {
                    var item = _this.homePageArr[i];
                AList.push(item);
            }
                _this.homePageArr = AList
        })
        },
        //取得輪播圖資料
        GetCarouselDetail() {
            let _this = this;
            axios.get("/Index/GetCarouselInfo").then(response => {
                _this.CarouselArr = response.data;
                let AList = []
                for (var i = 0; i < _this.CarouselArr.length; i++) {
                    var item = _this.CarouselArr[i];
                    AList.push(item);
                }
                _this.CarouselArr = AList
            })
        },
        //取得優惠公告資料
        GetAnnouncementDetail() {
            let _this = this;
            axios.get("/Index/GetAnnouncementInfo").then(response => {
                _this.AnnouncementArr = response.data;
                let AList = []
                for (var i = 0; i < _this.AnnouncementArr.length; i++) {
                    var item = _this.AnnouncementArr[i];
                    AList.push(item);
                }
                _this.AnnouncementArr = AList
            })
        },
        //取得熱賣資料
        GetHotSaleDetail() {
            let _this = this;
            axios.get("/Index/GetHotSaleInfo").then(response => {
                _this.HotSaleArr = response.data;
                let AList = []
                for (var i = 0; i < _this.HotSaleArr.length; i++) {
                    var item = _this.HotSaleArr[i];
                    AList.push(item);
                }
                _this.HotSaleArr = AList
            })
        },
    },
     mounted() {
         this.GethomePageDetail();
         this.GetCarouselDetail();
         this.GetAnnouncementDetail();
         this.GetHotSaleDetail();
    },
})              