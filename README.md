# Project Title
## ApplicationDBContext
```
using BusinessObject.Object;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext() { }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
        public virtual DbSet<Category> Categories { get; set; }

        public virtual DbSet<Member> Members { get; set; }

        public virtual DbSet<Order> Orders { get; set; }

        public virtual DbSet<OrderDetail> OrderDetails { get; set; }

        public virtual DbSet<Product> Products { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var config = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build();

                optionsBuilder.UseSqlServer(config.GetConnectionString("MyStoreDB"));

            }
        }


    }
}

```
## Vì sao cần DTO
vì dto sẽ tiết kiệm dữ liệu khi nó là class chỉ lấy ra những thuộc tính cần thiết
với API thì ta có Product class thì ta có ProductDTO, ProductCreateDTO, ProductUpdateDTO phục vụ cho mỗi mục đích khác nhau
## DAO là gì ?
DAO sẽ làm việc CRUD trực tiếp với context database
## Repository liên quan gì tới DAO ?
Repo sẽ gọi DAO thực hiện CRUD và có Interface quản lý bản thiết kế các hàm trong repo đó
"Nhờ ai đó làm việc gì" là việc của repo
## 3 layer architecture là gì ?
### Gồm 3 layer
**BusinessObject** chứa models và DBContext
**DataAccess** chứa DAO và Repository (ProductRepository và IProductRepository).
Khi **đi thi** muốn nhanh thì **bỏ qua DAO** và **dùng Repository generic** thay vì tạo từng repo cho từng class
### Set up Auto Mapper

B1: install **automapper.extensions.microsoftdepencyinjection**

B2: go to program and add code
```
builder.Services.AddAutoMapper(typeof(MappingConfig))
```
B3: create class **MappingConfig : Profile**
Nên dùng **ReverseMap()** để code ngắn và nhanh !

Thay vì như thế này
```
CreateMap<ProductDTO, ProductCreateDTO>()
CreateMap<ProductCreateDTO, ProductDTO>
```
thì sẽ như thế này
```
CreateMap<ProductDTO, ProductCreateDTO>().ReverseMap();
```
# API Project
## Standard API response
response API thường theo standard API response, không như bình thường chúng ta trả về một kiểu chay như trả về một đối tượng hay list

Tạo class **APIResponse**
```
public class APIResponse 
{ 
public HttpStatusCode StatusCode { get; set; } 
public bool IsSuccess { get; set; } 
public List<string> ErrorMessages { get; set; } 
public object Result { get; set; }
}
```
reponseAPI không phải service nên không được addscoped vào program. ĐIều đó dẫn đến việc khi khởi tạo không được cho vào tham số của contructor của class mà phải khởi tạo class
```
  protected APIResponse _response;
```
RỒi trong contructor khởi tạo lệnh `new()`
```
 _response = new();
```
## Addscoped và dbcontext khi làm với API vào program
1. Add DBContext, nếu không sẽ báo lỗi không có dbcontext

    1.1 Phải có appsettings với `copy always` properties và có ConnectionStrings
    ```
       "ConnectionStrings": {
   "MyStoreDB": "Data Source=(local);Database=Ass01_API;User Id=***;Password=***;TrustServerCertificate=true;Trusted_Connection=SSPI;Encrypt=false;"
 }
    ```
    1.2 Thêm DBcontext vào program :
    ```
    builder.Services.AddDbContext<ApplicationDbContext>(options =>
    {
        options.UseSqlServer(builder.Configuration.GetConnectionString("MyStoreDB"));
    });
    ```

2. AddScoped cho Repository và cả DAO nếu không sẽ lỗi lifetime
3. Có thể debug lỗi bằng cách thêm code trước `builder.build()`:
```
using (var scope = serviceProvider.CreateScope())
{
    var test = scope.ServiceProvider.GetService<IProductRepository>();
    if (test == null)
        Console.WriteLine("IProductRepository chưa được đăng ký đúng!");
}
```


## Publish Có Phải Là Hosting Không?
❌ Không!
Publish chỉ tạo file chạy, không tự động đưa lên web để mọi người truy cập.
Nếu muốn hosting (đưa lên internet), bạn cần deploy lên server như:
🌍 IIS (Windows Server)
🌐 Linux Server (Nginx, Apache, Kestrel)
☁️ Cloud (Azure, AWS, Heroku, DigitalOcean, etc.)
### Khi bạn chạy lệnh dotnet publish, .NET sẽ:
✅ Biên dịch toàn bộ code thành file thực thi (EXE, DLL).
✅ Copy tất cả dependencies (các thư viện cần thiết).
✅ Tùy chọn đóng gói runtime .NET (nếu Self-contained).
✅ Tạo thư mục "publish", chứa phiên bản chạy được của app.
Sau đó, bạn có thể mang thư mục này đi chạy trên server hoặc máy khác mà không cần code gốc.
## Deploy nhớ thêm dòng này để tránh lỗi
```
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}else{
options.RoutePrefix = " "
}
```

## Pushlish project nên chọn deploy mode nào ?
* Nếu triển khai trên server, cloud, hoặc máy có cài sẵn .NET → Chọn FDD (Framework-dependent) để tiết kiệm dung lượng.
* Nếu triển khai trên máy khách, thiết bị chưa cài .NET, hoặc muốn đơn giản hóa cài đặt → Chọn SCD (Self-contained) để đảm bảo chạy được ngay.
